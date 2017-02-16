using SmokSmog.Diagnostics;
using SmokSmog.Model;
using SmokSmog.Notification;
using SmokSmog.ViewModel;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SmokSmog.Tiles
{
    public class TilesUpdater
    {
        public async Task PrimaryTileRenderAndUpdate(CancellationToken token = default(CancellationToken))
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
                    if (token.IsCancellationRequested)
                    {
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                        return;
                    }

                    await RenderPrimaryTile(vm);
                    PrimaryTileUpdate(vm);
                }
                else
                {
                    // if station is invalid clear and return
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                }
            }
        }

        public void PrimaryTileUpdate(StationViewModel stationViewModel)
        {
            try
            {
                //TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

#if WINDOWS_UWP

                var template =
                    $"<tile><visual version=\"4\">" +
                    $"<binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{stationViewModel.Station.Name}\">" +
                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_0.png\"/>" +
                    $"</binding></visual></tile>";

#elif WINDOWS_APP || WINDOWS_PHONE_APP

                var template =
                    $"<tile><visual version=\"4\">" +
                    $"<binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{stationViewModel.Station.Name}\">" +
                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_0.png\"/>" +
                    $"<text id=\"2\">{stationViewModel.Station.Name}</text>" +
                    $"</binding></visual></tile>";
#endif

                //var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Image);
                var tileXml = new XmlDocument();
                tileXml.LoadXml(template);

                var tileImage = tileXml.GetElementsByTagName("image")[0] as XmlElement;
                //tileImage?.SetAttribute("src", "ms-appdata:///local/LiveTileFront_0.png");
                //tileXml.GetElementsByTagName()

                var date = stationViewModel.AirQualityIndex.DateUtc;
                var expiration = new DateTime(date.Year, date.Month, date.Day, date.Hour + 1, 30, 0, DateTimeKind.Utc);

                var tileNotification = new TileNotification(tileXml)
                {
                    Tag = "front",
                    ExpirationTime = new DateTimeOffset(expiration)
                };
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                if (stationViewModel.AirQualityIndex.Level != AirQualityLevel.NotAvailable)
                {
                    tileImage?.SetAttribute("src", "ms-appdata:///local/LiveTileBack_0.png");
                    tileNotification = new TileNotification(tileXml)
                    {
                        Tag = "back",
                        ExpirationTime = new DateTimeOffset(expiration)
                    };
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                }
            }
            finally
            {
                var tilesService = Services.ServiceLocator.Current.TilesService;
                tilesService.PrimaryTileLastUpdate = DateTime.Now;
            }
        }

        public async Task RenderPrimaryTile(StationViewModel stationViewModel)
        {
            using (TileRenderer tileRenderer = new TileRenderer())
            {
                try
                {
                    MemoryInfo.DebugMemoryStatus("Before Rendering Start");

                    await tileRenderer.RenderMediumTileFront($"LiveTileFront_0.png", stationViewModel);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    if (stationViewModel.AirQualityIndex.Level != AirQualityLevel.NotAvailable)
                    {
                        await tileRenderer.RenderMediumTileBack($"LiveTileBack_0.png", stationViewModel);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }

                    MemoryInfo.DebugMemoryStatus("After GC Collection");
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