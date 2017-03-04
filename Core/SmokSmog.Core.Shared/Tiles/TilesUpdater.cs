using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace SmokSmog.Tiles
{
    using Diagnostics;
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
                try
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

                        var files = await RenderPrimaryTile(vm);
                        PrimaryTileUpdate(vm, files);
                    }
                    else
                    {
                        // if station is invalid clear and return
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                    }
                }
                catch (Exception)
                {
                    // if station is invalid clear and return
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                    throw;
                }
            }

            stopwatch.Stop();
            Debug.WriteLine("Total Seconds Elapsed: {0}", stopwatch.ElapsedMilliseconds);
        }

        public void PrimaryTileUpdate(StationViewModel stationViewModel, Dictionary<TileSize, string[]> files)
        {
            try
            {
                // it is required to proper tiles refresh
                // sometimes Windows 10 without this don't update tile
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

                // calculate expiration time from local time (on UTC it does not work properly)
                var date = stationViewModel.AirQualityIndex.Date;
                date = date.AddHours(1);
                var expiration = new DateTime(date.Year, date.Month, date.Day, date.Hour, 30, 0);

                var tileXmls = BuildXmlTileUpdates(stationViewModel.Station.Name, files);

                for (int i = 0; i < tileXmls.Length; i++)
                {
                    var xml = tileXmls[i];
                    if (xml == null) return;

                    var tileNotification = new TileNotification(xml)
                    {
                        Tag = $"update{i}",
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

        private struct TileBindingProperties
        {
            public TileSize TileSize { get; set; }

            public string Template { get; set; }

            public string Fallback { get; set; }

            public string Branding { get; set; }
        }

        private static readonly TileBindingProperties[] TileBindingPropertiesArray = new[]
        {
            new TileBindingProperties { TileSize = TileSize.Medium, Branding = "name", Template = "TileSquare150x150Image", Fallback = "TileSquareImage"},
            new TileBindingProperties { TileSize = TileSize.Wide  , Branding = "name", Template = "TileWide310x150Image" , Fallback = "TileWideImage"},
            new TileBindingProperties { TileSize = TileSize.Large , Branding = "name", Template = "TileSquare310x310Image", Fallback = "TileSquareImage"},
        };

        public XmlDocument[] BuildXmlTileUpdates(string displayName, Dictionary<TileSize, string[]> filenames)
        {
            var result = new List<XmlDocument>();

            if (!filenames.Any())
                return result.ToArray();

            var count = filenames.Max(o => o.Value.Length);

            for (int i = 0; i < count; i++)
            {
                Dictionary<TileSize, string> filename = new Dictionary<TileSize, string>();

                foreach (var pair in filenames)
                {
                    if (pair.Value.Length > i)
                        filename[pair.Key] = pair.Value[i];
                }
                var xml = BuildXmlTileUpdate(displayName, filename);

                if (xml != null)
                    result.Add(xml);
            }

            return result.ToArray();
        }

        private XmlDocument BuildXmlTileUpdate(string displayName, Dictionary<TileSize, string> filenames)
        {
            //#if WINDOWS_UWP

            StringBuilder xmlTileUpdate = new StringBuilder();

            xmlTileUpdate.Append($"<tile><visual version=\"4\">");

            bool anyUpdate = false;

            foreach (var temp in TileBindingPropertiesArray)
            {
                string file = null;
                if (filenames.TryGetValue(temp.TileSize, out file) && !string.IsNullOrWhiteSpace(file))
                {
                    anyUpdate = true;

                    xmlTileUpdate.Append($"<binding template=\"{temp.Template}\" fallback=\"{temp.Fallback}\" ");
                    xmlTileUpdate.Append($" branding=\"{temp.Branding}\" displayName=\"{displayName}\">");
                    xmlTileUpdate.Append($"<image id=\"1\" src=\"ms-appdata:///local/{file}\"/>");
                    xmlTileUpdate.Append($"<text id=\"1\">{displayName}</text>");
                    xmlTileUpdate.Append($"</binding>");
                }
            }

            if (!anyUpdate)
                return null;

            xmlTileUpdate.Append($"</visual></tile>");
            var template = xmlTileUpdate.ToString();
            var tileXml = new XmlDocument();
            tileXml.LoadXml(template);
            return tileXml;

            //#elif WINDOWS_APP || WINDOWS_PHONE_APP

            //                // TODO - figure out how to show station name on primary tile
            //                var template =
            //                    $"<tile><visual version=\"4\">" +
            //                    $"<binding template=\"TileSquare150x150Image\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{displayName}\">" +
            //                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_0.png\"/>" +
            //                    $"<text id=\"2\">{displayName}</text>" +
            //                    $"</binding>" +
            //                    $"<binding template=\"TileWideImage\" fallback=\"TileSquareImage\" branding=\"name\" displayName=\"{displayName}\">" +
            //                    $"<image id=\"1\" src=\"ms-appdata:///local/LiveTileFront_1.png\"/>" +
            //                    $"<text id=\"2\">{displayName}</text>" +
            //                    $"</binding></visual></tile>";
            //#endif
        }

        public async Task<Dictionary<TileSize, string[]>> RenderPrimaryTile(StationViewModel stationViewModel)
        {
            try
            {
                using (TileRenderer tileRenderer = new TileRenderer())
                {
                    MemoryInfo.DebugMemoryStatus("Before Rendering");

                    string tileName = "PrimaryTile";
                    return await tileRenderer.RenderAllTiles(tileName, stationViewModel);
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
                MemoryInfo.DebugMemoryStatus("After Rendering");
                await MemoryInfo.SaveLog("BackgroundTask.Memory.log");
            }

            return new Dictionary<TileSize, string[]>();
        }
    }
}