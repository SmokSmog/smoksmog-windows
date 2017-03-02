using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using SmokSmog.Model;
using SmokSmog.ViewModel;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;

namespace SmokSmog.Notification
{
    internal class TileRenderer : IDisposable
    {
        private CanvasBitmap[] _bitmaps;

        private CanvasDevice _device;

        public TileRenderer()
        {
            _device = CanvasDevice.GetSharedDevice();
        }

        public void Dispose()
        {
            if (_bitmaps != null)
            {
                // free bitmap resources
                for (int i = 0; i < _bitmaps.Length; i++)
                {
                    _bitmaps[i].Dispose();
                    _bitmaps[i] = null;
                }
                _bitmaps = null;
            }

            _device?.Dispose();
            _device = null;
            MemoryInfo.DebugMemoryStatus("After release device");
        }

        public async Task RenderMediumTileBackAsync(string filename, StationViewModel stationViewModel, float dpi)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            if (stationViewModel == null)
                throw new ArgumentNullException(nameof(stationViewModel));

            MemoryInfo.DebugMemoryStatus("Start Rendering Medium Tile");
            await LoadBitmapsAsync();
            MemoryInfo.DebugMemoryStatus("After loading Bitmaps");
            var size = new Size(150, 150);

            using (var renderTarget = new CanvasRenderTarget(_device, (float)size.Width, (float)size.Height, dpi))
            {
                MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                using (var session = renderTarget.CreateDrawingSession())
                {
                    session.Clear(Colors.Transparent);

                    MemoryInfo.DebugMemoryStatus("After creating session");

                    for (var i = 0; i < 2 && i < stationViewModel.AqiComponents.Count; i++)
                    {
                        var model = stationViewModel.AqiComponents[i];
                        RenderSubGroupAsync(session,
                            _bitmaps[(int)model.AirQualityIndex.Level],
                            model.Parameter.ShortName,
                            model.Latest.Avg1Hour?.ToString("0.0"),
                            model.Parameter.Unit,
                            i * 75,
                            0);
                    }
                    //RenderSubGroup(session, bitmaps[0], "PM₁₀", "142.0", "µg/m³", 0, 0);
                    //RenderSubGroup(session, bitmaps[0], "PM₁₀", "142.0", "µg/m³", 150, 0);
                    session.Dispose();
                }
                MemoryInfo.DebugMemoryStatus("After end session");

                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
                await renderTarget.SaveAsync(path, CanvasBitmapFileFormat.Png);
                MemoryInfo.DebugMemoryStatus("After save renderTarget");
                renderTarget.Dispose();
            }
            MemoryInfo.DebugMemoryStatus("After release renderTarget");
        }

        public async Task RenderMediumTileFrontAsync(string filename, StationViewModel stationViewModel, float dpi)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            if (stationViewModel == null)
                throw new ArgumentNullException(nameof(stationViewModel));

            MemoryInfo.DebugMemoryStatus("Start Rendering Medium Tile");
            await LoadBitmapsAsync();
            MemoryInfo.DebugMemoryStatus("After loading Bitmaps");
            var size = new Size(150, 150);

            using (var renderTarget = new CanvasRenderTarget(_device, (float)size.Width, (float)size.Height, dpi))
            {
                MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                using (var session = renderTarget.CreateDrawingSession())
                {
                    session.Clear(Colors.Transparent);

                    MemoryInfo.DebugMemoryStatus("After creating session");

                    using (CanvasTextFormat textFromat = new CanvasTextFormat
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 20,
                        HorizontalAlignment = CanvasHorizontalAlignment.Left,
                        VerticalAlignment = CanvasVerticalAlignment.Center,
                    })
                    {
                        MemoryInfo.DebugMemoryStatus("Start Drawing SbuGroup");

                        if (stationViewModel.AirQualityIndex.Level != AirQualityLevel.NotAvailable)
                            session.DrawImage(_bitmaps[(int)stationViewModel.AirQualityIndex.Level], new Rect(35, 35, 80, 80));

                        session.DrawText(stationViewModel.AirQualityIndex.Text, new Rect(10, 10, 150, 25), Colors.White, textFromat);

                        MemoryInfo.DebugMemoryStatus("End Drawing SbuGroup");

                        textFromat.Dispose();
                    }
                    session.Dispose();
                }
                MemoryInfo.DebugMemoryStatus("After end session");

                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
                await renderTarget.SaveAsync(path, CanvasBitmapFileFormat.Png);
                MemoryInfo.DebugMemoryStatus("After save renderTarget");
                renderTarget.Dispose();
            }
            MemoryInfo.DebugMemoryStatus("After release renderTarget");
        }

        public async Task RenderWideTileBackAsync(string filename, StationViewModel stationViewModel, float dpi)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            if (stationViewModel == null)
                throw new ArgumentNullException(nameof(stationViewModel));

            MemoryInfo.DebugMemoryStatus("Start Rendering Wide Tile");
            await LoadBitmapsAsync();
            MemoryInfo.DebugMemoryStatus("After loading Bitmaps");
            var size = new Size(310, 150);

            using (var renderTarget = new CanvasRenderTarget(_device, (float)size.Width, (float)size.Height, dpi))
            {
                MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                using (var session = renderTarget.CreateDrawingSession())
                {
                    session.Clear(Colors.Transparent);

                    MemoryInfo.DebugMemoryStatus("After creating session");

                    for (var i = 0; i < 4 && i < stationViewModel.AqiComponents.Count; i++)
                    {
                        var model = stationViewModel.AqiComponents[i];
                        RenderSubGroupAsync(session,
                            _bitmaps[(int)model.AirQualityIndex.Level],
                            model.Parameter.ShortName,
                            model.Latest.Avg1Hour?.ToString("0.0"),
                            model.Parameter.Unit,
                            5 + i * 75,
                            0);
                    }
                    session.Dispose();
                }
                MemoryInfo.DebugMemoryStatus("After end session");

                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
                await renderTarget.SaveAsync(path, CanvasBitmapFileFormat.Png);
                MemoryInfo.DebugMemoryStatus("After save renderTarget");
                renderTarget.Dispose();
            }
            MemoryInfo.DebugMemoryStatus("After release renderTarget");
        }

        public async Task RenderWideTileFrontAsync(string filename, StationViewModel stationViewModel, float dpi)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            if (stationViewModel == null)
                throw new ArgumentNullException(nameof(stationViewModel));

            MemoryInfo.DebugMemoryStatus("Start Rendering Wide Tile");
            await LoadBitmapsAsync();
            MemoryInfo.DebugMemoryStatus("After loading Bitmaps");
            var size = new Size(310, 150);

            using (var renderTarget = new CanvasRenderTarget(_device, (float)size.Width, (float)size.Height, dpi))
            {
                MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                using (var session = renderTarget.CreateDrawingSession())
                {
                    session.Clear(Colors.Transparent);

                    MemoryInfo.DebugMemoryStatus("After creating session");

                    using (CanvasTextFormat textFromat = new CanvasTextFormat
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 24f,
                        HorizontalAlignment = CanvasHorizontalAlignment.Left,
                        VerticalAlignment = CanvasVerticalAlignment.Center,
                    })
                    {
                        MemoryInfo.DebugMemoryStatus("Start Drawing SbuGroup");

                        var aqi = stationViewModel.AirQualityIndex;

                        if (aqi.Level != AirQualityLevel.NotAvailable)
                            session.DrawImage(_bitmaps[(int)aqi.Level], new Rect(20d, 20d, 80d, 80d));

                        session.DrawText(aqi.Text,
                            new Rect(110d, 20d, 200d, 26.6d), Colors.White, textFromat);

                        //DateTime date = DateTime.Now;
                        session.DrawText(aqi.Date.ToString("dd.MM.yy HH:mm"),
                            new Rect(110d, (20d + 26.6d), 200d, 26.6d), Colors.White, textFromat);

                        //session.DrawText(date.ToString("ddd"), new Rect(150, 170, 150, 50), Colors.White, textFromat);
                        session.DrawText(aqi.Date.ToString($"AQI : {aqi.Value:0.0}"),
                            new Rect(110d, (20d + 2 * 26.6d), 200d, 26.6d), Colors.White, textFromat);

                        MemoryInfo.DebugMemoryStatus("End Drawing SbuGroup");

                        textFromat.Dispose();
                    }
                    session.Dispose();
                }
                MemoryInfo.DebugMemoryStatus("After end session");

                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
                await renderTarget.SaveAsync(path, CanvasBitmapFileFormat.Png);
                MemoryInfo.DebugMemoryStatus("After save renderTarget");
                renderTarget.Dispose();
            }
            MemoryInfo.DebugMemoryStatus("After release renderTarget");
        }

        public async Task RenderLargeTileFrontAsync(string filename, StationViewModel stationViewModel, float dpi)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));

            if (stationViewModel == null)
                throw new ArgumentNullException(nameof(stationViewModel));

            MemoryInfo.DebugMemoryStatus("Start Rendering Wide Tile");
            await LoadBitmapsAsync();
            MemoryInfo.DebugMemoryStatus("After loading Bitmaps");
            var size = new Size(310, 310);

            using (var renderTarget = new CanvasRenderTarget(_device, (float)size.Width, (float)size.Height, dpi))
            {
                MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                using (var session = renderTarget.CreateDrawingSession())
                {
                    session.Clear(Colors.Transparent);

                    MemoryInfo.DebugMemoryStatus("After creating session");

                    using (CanvasTextFormat textFromat = new CanvasTextFormat
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 26,
                        HorizontalAlignment = CanvasHorizontalAlignment.Left,
                        VerticalAlignment = CanvasVerticalAlignment.Center,
                    })
                    {
                        MemoryInfo.DebugMemoryStatus("Start Drawing SbuGroup");

                        var aqi = stationViewModel.AirQualityIndex;

                        if (aqi.Level != AirQualityLevel.NotAvailable)
                            session.DrawImage(_bitmaps[(int)aqi.Level], new Rect(20d, 20d, 80d, 80d));

                        session.DrawText(aqi.Text,
                            new Rect(110d, 20d, 200d, 26.6d), Colors.White, textFromat);

                        //DateTime date = DateTime.Now;
                        session.DrawText(aqi.Date.ToString("dd.MM.yy HH:mm"),
                            new Rect(110d, (20d + 26.6d), 200d, 26.6d), Colors.White, textFromat);

                        //session.DrawText(date.ToString("ddd"), new Rect(150, 170, 150, 50), Colors.White, textFromat);
                        session.DrawText(aqi.Date.ToString($"AQI : {aqi.Value:0.0}"),
                            new Rect(110d, (20d + 2 * 26.6d), 200d, 26.6d), Colors.White, textFromat);

                        MemoryInfo.DebugMemoryStatus("End Drawing SbuGroup");

                        textFromat.Dispose();
                    }

                    for (var i = 0; i < 4 && i < stationViewModel.AqiComponents.Count; i++)
                    {
                        var model = stationViewModel.AqiComponents[i];
                        RenderSubGroupAsync(session,
                            _bitmaps[(int)model.AirQualityIndex.Level],
                            model.Parameter.ShortName,
                            model.Latest.Avg1Hour?.ToString("0.0"),
                            model.Parameter.Unit,
                            5 + i * 75,
                            140);
                    }

                    session.Dispose();
                }
                MemoryInfo.DebugMemoryStatus("After end session");

                var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
                await renderTarget.SaveAsync(path, CanvasBitmapFileFormat.Png);
                MemoryInfo.DebugMemoryStatus("After save renderTarget");
                renderTarget.Dispose();
            }
            MemoryInfo.DebugMemoryStatus("After release renderTarget");
        }

        private async Task LoadBitmapsAsync()
        {
            if (_bitmaps != null) return;
            _bitmaps = new[]
            {
                await CanvasBitmap.LoadAsync(_device, new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/VeryGood-square.png")),
                await CanvasBitmap.LoadAsync(_device, new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Good-square.png")),
                await CanvasBitmap.LoadAsync(_device, new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Moderate-square.png")),
                await CanvasBitmap.LoadAsync(_device, new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Sufficient-square.png")),
                await CanvasBitmap.LoadAsync(_device, new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Bad-square.png")),
                await CanvasBitmap.LoadAsync(_device, new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/VeryBad-square.png")),
            };

            MemoryInfo.DebugMemoryStatus("After loading Bitmaps");
        }

        private void RenderSubGroupAsync(CanvasDrawingSession session, CanvasBitmap bitmap, string text1, string text2, string text3, float shiftX, float shiftY)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));
            if (text1 == null) throw new ArgumentNullException(nameof(text1));
            if (text2 == null) throw new ArgumentNullException(nameof(text2));
            if (text3 == null) throw new ArgumentNullException(nameof(text3));

            using (var textFromat = new CanvasTextFormat
            {
                FontFamily = "Segoe UI",
                FontSize = 20,
                HorizontalAlignment = CanvasHorizontalAlignment.Center,
                VerticalAlignment = CanvasVerticalAlignment.Center
            })
            {
                MemoryInfo.DebugMemoryStatus("Start Drawing SbuGroup");

                session.DrawText(text1, new Rect(shiftX + 7.5, shiftY + 10, 60, 25), Colors.White, textFromat);
                session.DrawImage(bitmap, new Rect(shiftX + 7.5 + 10, shiftY + 30, 40, 40));
                session.DrawText(text2, new Rect(shiftX + 7.5, shiftY + 65, 60, 25), Colors.White, textFromat);
                session.DrawText(text3, new Rect(shiftX + 7.5, shiftY + 90, 60, 25), Colors.LightGray, textFromat);

                MemoryInfo.DebugMemoryStatus("End Drawing SbuGroup");

                textFromat.Dispose();
            }
        }
    }
}