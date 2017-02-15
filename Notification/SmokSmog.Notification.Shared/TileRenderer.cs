﻿using Microsoft.Graphics.Canvas;
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

        public async Task RenderMediumTileBack(string filename, StationViewModel stationViewModel)
        {
            MemoryInfo.DebugMemoryStatus("Start Rendering Medium Tile");

            await LoadBitmaps();

            // scale-200
            var size = new Size(300, 300);

            using (var renderTarget = new CanvasRenderTarget(_device, (float)size.Width, (float)size.Height, 96f))
            {
                MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                using (var session = renderTarget.CreateDrawingSession())
                {
                    MemoryInfo.DebugMemoryStatus("After creating session");

                    for (var i = 0; i < 2 && i < stationViewModel.AqiComponents.Count; i++)
                    {
                        var model = stationViewModel.AqiComponents[i];
                        RenderSubGroup(session,
                            _bitmaps[(int)model.AirQualityIndex.Level],
                            model.Parameter.ShortName,
                            model.Latest.Avg1Hour?.ToString("0.0"),
                            model.Parameter.Unit,
                            i * 150,
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

        public async Task RenderMediumTileFront(string filename, StationViewModel stationViewModel)
        {
            MemoryInfo.DebugMemoryStatus("Start Rendering Medium Tile");

            await LoadBitmaps();

            MemoryInfo.DebugMemoryStatus("After loading Bitmaps");

            // scale-200
            var size = new Size(300, 300);

            using (var renderTarget = new CanvasRenderTarget(_device, (float)size.Width, (float)size.Height, 96f))
            {
                MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                using (var session = renderTarget.CreateDrawingSession())
                {
                    MemoryInfo.DebugMemoryStatus("After creating session");

                    using (CanvasTextFormat textFromat = new CanvasTextFormat
                    {
                        FontFamily = "Segoe UI",
                        FontSize = 40,
                        HorizontalAlignment = CanvasHorizontalAlignment.Left,
                        VerticalAlignment = CanvasVerticalAlignment.Center,
                    })
                    {
                        MemoryInfo.DebugMemoryStatus("Start Drawing SbuGroup");

                        if (stationViewModel.AirQualityIndex.Level != AirQualityLevel.NotAvailable)
                            session.DrawImage(_bitmaps[(int)stationViewModel.AirQualityIndex.Level], new Rect(70, 70, 160, 160));

                        session.DrawText(stationViewModel.AirQualityIndex.Text, new Rect(20, 20, 300, 50), Colors.White, textFromat);

                        //DateTime date = DateTime.Now;
                        //session.DrawText(date.ToString("HH:mm"), new Rect(150, 100, 150, 50), Colors.White, textFromat);
                        //session.DrawText(date.ToString("ddd"), new Rect(150, 170, 150, 50), Colors.White, textFromat);

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

        private async Task LoadBitmaps()
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

        private void RenderSubGroup(CanvasDrawingSession session, CanvasBitmap bitmap, string text1, string text2, string text3, float shiftX, float shiftY)
        {
            using (var textFromat = new CanvasTextFormat
            {
                FontFamily = "Segoe UI",
                FontSize = 38,
                HorizontalAlignment = CanvasHorizontalAlignment.Center,
                VerticalAlignment = CanvasVerticalAlignment.Center
            })
            {
                MemoryInfo.DebugMemoryStatus("Start Drawing SbuGroup");

                session.DrawText(text1, new Rect(shiftX + 15, shiftY + 20, 120, 50), Colors.White, textFromat);
                session.DrawImage(bitmap, new Rect(shiftX + 15 + 20, shiftY + 70 - 10, 80, 80));
                session.DrawText(text2, new Rect(shiftX + 15, shiftY + 130, 120, 50), Colors.White, textFromat);
                session.DrawText(text3, new Rect(shiftX + 15, shiftY + 180, 120, 50), Colors.LightGray, textFromat);

                MemoryInfo.DebugMemoryStatus("End Drawing SbuGroup");

                textFromat.Dispose();
            }
        }
    }
}