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

            // just for test create file
            await ApplicationData.Current.LocalFolder.CreateFileAsync("test.txt", CreationCollisionOption.ReplaceExisting);

            await RenderAndUpdate();

            // Inform the system that the task is finished.
            _deferral.Complete();
        }

        public IAsyncAction RunAction(bool renderOnly)
        {
            return RenderAndUpdate(renderOnly).AsAsyncAction();
        }

        internal async Task RenderAndUpdate(bool renderOnly = false)
        {
            var stationId = Settings.HomeStationId ?? (int?)4;
            if (stationId.HasValue)
            {
                var vm = new StationViewModel();
                await vm.SetStationAsync(stationId.Value);

                if (vm.IsValidStation)
                {
                    await RenderTiles(vm);
                    UpdateTile(vm);
                }
            }
        }

        internal void UpdateTile(StationViewModel stationViewModel)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Image);

            var tileImage = tileXml.GetElementsByTagName("image")[0] as XmlElement;
            tileImage.SetAttribute("src", "ms-appdata:///local/LiveTileFront_0.png");
            var tileNotification = new TileNotification(tileXml) { Tag = "front" };
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

            tileImage.SetAttribute("src", "ms-appdata:///local/LiveTileBack_0.png");
            tileNotification = new TileNotification(tileXml) { Tag = "back" };
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
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
                await MemoryInfo.SaveLog();
            }
        }
    }
}