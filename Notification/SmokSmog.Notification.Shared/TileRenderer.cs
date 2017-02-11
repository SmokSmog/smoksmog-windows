﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.System;
using Windows.UI;

namespace SmokSmog.Notification
{
    internal class MemoryInfo
    {
        public static string MemoryStatus()
        {
            var memory = MemoryManager.AppMemoryUsage;
            var memoryLimit = MemoryManager.AppMemoryUsageLimit;
            return $"Memory : \n\tused {ToMegaBytes(memory)} with limit {ToMegaBytes(memoryLimit)} MB\n\tused {ToKiloBytes(memory)} with limit {ToKiloBytes(memoryLimit)} KB";
        }

        private static StringBuilder sb = new StringBuilder();

        public static void DebugMemoryStatus(string str, params object[] paraeters)
        {
            sb.AppendFormat(str, paraeters);
            Debug.WriteLine(str, paraeters);

            var meminfo = MemoryInfo.MemoryStatus();
            sb.AppendLine(meminfo);
            Debug.WriteLine(meminfo);
        }

        public static async Task SaveLog()
        {
            var log = sb.ToString();
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync("BackGroundTask.log", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, log);
        }

        private static float ToMegaBytes(ulong memory)
        {
            return memory / 1024f / 1024f;
        }

        private static float ToKiloBytes(ulong memory)
        {
            return memory / 1024f;
        }
    }

    internal class TileRenderer
    {
        public static async Task RenderMediumTile(string filename)
        {
            MemoryInfo.DebugMemoryStatus("Start Rendering Medium Tile");

            using (var device = CanvasDevice.GetSharedDevice())
            {
                Uri[] imageUris = new[]
                {
                    new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/VeryGood-square.png"),
                    new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Good-square.png"),
                    new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Moderate-square.png"),
                    new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Sufficient-square.png"),
                    new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Bad-square.png"),
                    new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/VeryBad-square.png"),
                };

                CanvasBitmap[] bitmaps = new[]
                {
                    await CanvasBitmap.LoadAsync(device, imageUris[0]),
                    //await CanvasBitmap.LoadAsync(device, imageUris[1]),
                    //await CanvasBitmap.LoadAsync(device, imageUris[2]),
                    //await CanvasBitmap.LoadAsync(device, imageUris[3]),
                    //await CanvasBitmap.LoadAsync(device, imageUris[4]),
                    //await CanvasBitmap.LoadAsync(device, imageUris[5]),
                };

                MemoryInfo.DebugMemoryStatus("After loading Bitmaps");

                // scale-200
                var size = new Size(300, 300);

                using (var renderTarget = new CanvasRenderTarget(device, (float)size.Width, (float)size.Height, 96f))
                {
                    MemoryInfo.DebugMemoryStatus("After creating renderTarget");

                    using (var session = renderTarget.CreateDrawingSession())
                    {
                        MemoryInfo.DebugMemoryStatus("After creating session");
                        RenderSubGroup(session, bitmaps[0], "PM₁₀", "142.0", "µg/m³", 0, 0);
                        RenderSubGroup(session, bitmaps[0], "PM₁₀", "142.0", "µg/m³", 150, 0);
                        bitmaps[0].Dispose();
                        session.Dispose();
                    }
                    MemoryInfo.DebugMemoryStatus("After end session");

                    var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
                    await renderTarget.SaveAsync(path, CanvasBitmapFileFormat.Png);
                    MemoryInfo.DebugMemoryStatus("After save renderTarget");
                    renderTarget.Dispose();
                }
                MemoryInfo.DebugMemoryStatus("After release renderTarget");

                // free bitmap resources
                for (int i = 0; i < bitmaps.Length; i++)
                {
                    bitmaps[i].Dispose();
                    bitmaps[i] = null;
                }
                bitmaps = null;
                device.Dispose();
            }
            MemoryInfo.DebugMemoryStatus("After release device");
        }

        private static void RenderSubGroup(CanvasDrawingSession session, CanvasBitmap bitmap, string text1, string text2, string text3, float shiftX, float shiftY)
        {
            using (var textFromat = new CanvasTextFormat
            {
                FontFamily = "Segoe UI",
                FontSize = 36,
                HorizontalAlignment = CanvasHorizontalAlignment.Center,
                VerticalAlignment = CanvasVerticalAlignment.Center
            })
            {
                MemoryInfo.DebugMemoryStatus("Start Drawing SbuGroup");

                session.DrawText(text1, new Rect(shiftX + 15, shiftY + 20, 120, 50), Colors.White, textFromat);
                session.DrawImage(bitmap, new Rect(shiftX + 15 + 20, shiftY + 70 - 10, 80, 80));
                session.DrawText(text2, new Rect(shiftX + 15, shiftY + 130, 120, 50), Colors.White, textFromat);
                session.DrawText(text3, new Rect(shiftX + 15, shiftY + 180, 120, 50), Colors.DarkGray, textFromat);

                MemoryInfo.DebugMemoryStatus("End Drawing SbuGroup");

                textFromat.Dispose();
            }
        }
    }
}