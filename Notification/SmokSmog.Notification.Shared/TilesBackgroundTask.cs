using SmokSmog.Diagnostics;
using SmokSmog.ViewModel;
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
    public sealed class TilesBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely while asynchronous code
            // is still running.
            _deferral = taskInstance.GetDeferral();

            var log = await ApplicationData.Current.LocalFolder.CreateFileAsync("BackgroundTask.Execution.log", CreationCollisionOption.ReplaceExisting);
            await FileIO.AppendTextAsync(log, $"\"Start\": \"{DateTime.Now:G}\",");

            await PrimaryTileRenderAndUpdate();

            await FileIO.AppendTextAsync(log, $"\"End\" : \"{DateTime.Now:G}\"");
            // Inform the system that the task is finished.
            _deferral.Complete();
        }

        public IAsyncAction RunAction(bool renderOnly)
        {
            return PrimaryTileRenderAndUpdate(renderOnly).AsAsyncAction();
        }

        internal async Task PrimaryTileRenderAndUpdate(bool renderOnly = false)
        {
            // if primary tile is disabled by user in application settings
            // clear tile and return
            if (!TilesService.Current.IsPrimaryTileNotificationEnable)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                return;
            }

            var stationId = Settings.Current.HomeStationId;
            if (stationId.HasValue)
            {
                //Download data (Set will download data from server)
                var vm = new StationViewModel();
                await vm.SetStationAsync(stationId.Value);

                if (vm.IsValidStation)
                {
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

                var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Image);

                var tileImage = tileXml.GetElementsByTagName("image")[0] as XmlElement;
                tileImage.SetAttribute("src", "ms-appdata:///local/LiveTileFront_0.png");
                var tileNotification = new TileNotification(tileXml)
                {
                    Tag = "front",
                    ExpirationTime = DateTimeOffset.Now.AddHours(2)
                };
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

                tileImage.SetAttribute("src", "ms-appdata:///local/LiveTileBack_0.png");
                tileNotification = new TileNotification(tileXml)
                {
                    Tag = "back",
                    ExpirationTime = DateTimeOffset.Now.AddHours(2)
                };
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            finally
            {
                TilesService.Current.PrimaryTileLastUpdate = DateTime.Now;
            }
        }

        internal async Task RenderTiles(StationViewModel stationViewModel)
        {
            TileRenderer tileRenderer = new TileRenderer();

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
                await MemoryInfo.SaveLog("BackgroundTask.Memory.log");
            }
        }
    }
}