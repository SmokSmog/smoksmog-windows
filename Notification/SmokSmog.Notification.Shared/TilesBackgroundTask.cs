using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Notifications;

namespace SmokSmog.Notification
{
    using Diagnostics;
    using ViewModel;

    public sealed class TilesBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        private BackgroundTaskDeferral _deferral;
        private volatile bool _cancelRequested = false;

        /// <summary>
        /// Primary Tile Updater Background Task
        /// </summary>
        /// <seealso cref="https://code.msdn.microsoft.com/windowsapps/Background-Task-Sample-9209ade9"/>
        /// <param name="taskInstance"></param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely while asynchronous code
            // is still running.
            _deferral = taskInstance.GetDeferral();

            // Query BackgroundWorkCost
            // Guidance: If BackgroundWorkCost is high, then perform only the minimum amount
            // of work in the background task and return immediately.
            var log = await ApplicationData.Current.LocalFolder.CreateFileAsync("BackgroundTask.Execution.log", CreationCollisionOption.ReplaceExisting);
            await FileIO.AppendTextAsync(log, $"\"Start\": \"{DateTime.Now:G}\", " +
                                              $"\"Cost\": \"{BackgroundWorkCost.CurrentBackgroundWorkCost}\"");

            // Associate a cancellation handler with the background task.
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

            await PrimaryTileRenderAndUpdate();

            await FileIO.AppendTextAsync(log, $"\"End\" : \"{DateTime.Now:G}\"");
            // Inform the system that the task is finished.
            _deferral.Complete();
        }

        // Handles background task cancellation.
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //
            // Indicate that the background task is canceled.
            //
            _cancelRequested = true;
            _cancelReason = reason;

            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }

        public IAsyncAction RunAction(bool renderOnly)
        {
            return PrimaryTileRenderAndUpdate(renderOnly).AsAsyncAction();
        }

        internal async Task PrimaryTileRenderAndUpdate(bool renderOnly = false)
        {
            var tilesService = Services.ServiceLocator.Current.TilesService;
            var settingsService = Services.ServiceLocator.Current.SettingsService;

            // if primary tile is disabled by user in application settings
            // clear tile and return
            if (!tilesService.IsPrimaryTileNotificationEnable)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                return;
            }

            var stationId = settingsService.HomeStationId;
            if (stationId.HasValue)
            {
                //Download data (Set will download data from server)
                var vm = new StationViewModel();
                await vm.SetStationAsync(stationId.Value);

                if (vm.IsValidStation)
                {
                    if (_cancelRequested)
                    {
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                        return;
                    }

                    await RenderTiles(vm);
                    UpdateTile(vm);
                }
                else
                {
                    // if station is invalid clear and return
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                }
            }
        }

        internal void UpdateTile(StationViewModel stationViewModel)
        {
            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

#if WINDOWS_UWP

                var template =
                    $"<tile><visual version=\"4\">" +
                    $"<binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\" branding=\"nameAndLogo\" displayName=\"{stationViewModel.Station.Name}\">" +
                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_0.png\"/>" +
                    $"</binding></visual></tile>";

#elif WINDOWS_APP || WINDOWS_PHONE_APP

                var template =
                    $"<tile><visual version=\"4\">" +
                    $"<binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\" branding=\"logo\" displayName=\"{stationViewModel.Station.Name}\">" +
                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_0.png\"/>" +
                    $"</binding></visual></tile>";
#endif

                //var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Image);
                var tileXml = new XmlDocument();
                tileXml.LoadXml(template);

                var tileImage = tileXml.GetElementsByTagName("image")[0] as XmlElement;
                //tileImage?.SetAttribute("src", "ms-appdata:///local/LiveTileFront_0.png");
                //tileXml.GetElementsByTagName()

                var tileNotification = new TileNotification(tileXml)
                {
                    Tag = "front",
                    ExpirationTime = DateTimeOffset.Now.AddHours(2)
                };
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

                tileImage?.SetAttribute("src", "ms-appdata:///local/LiveTileBack_0.png");
                tileNotification = new TileNotification(tileXml)
                {
                    Tag = "back",
                    ExpirationTime = DateTimeOffset.Now.AddHours(2)
                };
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            finally
            {
                var tilesService = Services.ServiceLocator.Current.TilesService;
                tilesService.PrimaryTileLastUpdate = DateTime.Now;
            }
        }

        internal async Task RenderTiles(StationViewModel stationViewModel)
        {
            using (TileRenderer tileRenderer = new TileRenderer())
            {
                try
                {
                    for (int i = 0; i < 1; i++)
                    {
                        MemoryInfo.DebugMemoryStatus("Before Rendering Start");

                        await tileRenderer.RenderMediumTileBack($"LiveTileBack_{i}.png", stationViewModel);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();

                        await tileRenderer.RenderMediumTileFront($"LiveTileFront_{i}.png", stationViewModel);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();

                        MemoryInfo.DebugMemoryStatus("After GC Collection");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    if (Debugger.IsAttached)
                        Debugger.Break();
                }
                finally
                {
                    tileRenderer.Dispose();
                    await MemoryInfo.SaveLog("BackgroundTask.Memory.log");
                }
            }
        }
    }
}