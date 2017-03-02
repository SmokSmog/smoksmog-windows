using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SmokSmog.Tiles
{
    using Diagnostics;
    using Model;
    using Notification;
    using ViewModel;

    public class TilesUpdater
    {
        public async Task PrimaryTileRenderAndUpdate(CancellationToken token = default(CancellationToken))
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

            stopwatch.Stop();
            Debug.WriteLine("Total Seconds Elapsed: {0}", stopwatch.ElapsedMilliseconds);
        }

        public void PrimaryTileUpdate(StationViewModel stationViewModel)
        {
            try
            {
                // it is required to proper tiles refresh
                // sometimes Windows 10 without this don't update tile
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

#if WINDOWS_UWP

                StringBuilder xmlTileUpdate = new StringBuilder();

                xmlTileUpdate.Append($"<tile><visual version=\"4\">");

                xmlTileUpdate.Append($"<binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{stationViewModel.Station.Name}\">");
                xmlTileUpdate.Append($"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_0.png\"/>");
                xmlTileUpdate.Append($"</binding>");

                xmlTileUpdate.Append($"<binding template=\"TileWide310x150Image\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{stationViewModel.Station.Name}\">");
                xmlTileUpdate.Append($"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_1.png\"/>");
                xmlTileUpdate.Append($"</binding>");

                xmlTileUpdate.Append($"<binding template=\"TileSquare310x310Image\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{stationViewModel.Station.Name}\">");
                xmlTileUpdate.Append($"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_2.png\"/>");
                xmlTileUpdate.Append($"</binding>");

                xmlTileUpdate.Append($"</visual></tile>");

                var template = xmlTileUpdate.ToString();

#elif WINDOWS_APP || WINDOWS_PHONE_APP

                // TODO - figure out how to show station name on primary tile
                var template =
                    $"<tile><visual version=\"4\">" +
                    $"<binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{stationViewModel.Station.Name}\">" +
                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_0.png\"/>" +
                    $"<text id=\"2\">{stationViewModel.Station.Name}</text>" +
                    $"</binding>" +
                    $"<binding template=\"TileWideImage\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{stationViewModel.Station.Name}\">" +
                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_1.png\"/>" +
                    $"<text id=\"2\">{stationViewModel.Station.Name}</text>" +
                    $"</binding></visual></tile>";
#endif

                //var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Image);
                var tileXml = new XmlDocument();
                tileXml.LoadXml(template);

                // calculate expiration time from local time (on UTC it does not work properly)
                var date = stationViewModel.AirQualityIndex.Date;
                date = date.AddHours(1);
                var expiration = new DateTime(date.Year, date.Month, date.Day, date.Hour, 30, 0);

                var tileNotification = new TileNotification(tileXml)
                {
                    Tag = "front",
                    ExpirationTime = new DateTimeOffset(expiration)
                };
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

                if (stationViewModel.AirQualityIndex.Level != AirQualityLevel.NotAvailable)
                {
                    var tile150x150Image = tileXml.GetElementsByTagName("image")[0] as XmlElement;
                    tile150x150Image?.SetAttribute("src", "ms-appdata:///local/LiveTileBack_0.png");

                    var tile310x150Image = tileXml.GetElementsByTagName("image")[1] as XmlElement;
                    tile310x150Image?.SetAttribute("src", "ms-appdata:///local/LiveTileBack_1.png");

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

                    //float dpi = 96f; // 100%
                    float dpi = 192f; // 200%

                    await tileRenderer.RenderMediumTileFrontAsync($"LiveTileFront_0.png", stationViewModel, dpi);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    await tileRenderer.RenderWideTileFrontAsync($"LiveTileFront_1.png", stationViewModel, dpi);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    await tileRenderer.RenderLargeTileFrontAsync($"LiveTileFront_2.png", stationViewModel, dpi);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    if (stationViewModel.AirQualityIndex.Level != AirQualityLevel.NotAvailable)
                    {
                        await tileRenderer.RenderMediumTileBackAsync($"LiveTileBack_0.png", stationViewModel, dpi);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();

                        await tileRenderer.RenderWideTileBackAsync($"LiveTileBack_1.png", stationViewModel, dpi);
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