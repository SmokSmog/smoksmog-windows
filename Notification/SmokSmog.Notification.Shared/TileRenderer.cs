using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SmokSmog.Notification
{
    internal class TileRenderer
    {
        public static async Task CreateTileGraphAsync(string data, int width, int height, string filename)
        {
            var control = await GetVisual(data, width, height);
            await RenderAndSaveToFileAsync(control, (uint)width, (uint)height, filename);
        }

        public static async Task CreateTileGraphAsync(string data, int width, int height, IRandomAccessStream stream)
        {
            var control = await GetVisual(data, width, height);
            await RenderAndSaveToStreamAsync(control, (uint)width, (uint)height, stream);
        }

        public static async Task<UIElement> GetVisual(string data, int width, int height)
        {
            return await GetVisual(data, width, height, new SolidColorBrush(Colors.White));
        }

        public static async Task<UIElement> GetVisual(string data, int width, int height, Brush pathbrush)
        {
            ////TODO: Create you XAML UI here and return root element
            //return new Border()
            //{
            //    Width = width,
            //    Height = height,
            //    Background = pathbrush
            //};

            //var resource = new ResourceDictionary();
            //var uri = new Uri("ms-appx:///SmokSmog.Core/ThemesThemes/Generic.xaml", UriKind.Absolute);
            var uri = new Uri("ms-appx:///SmokSmog.Core/Tiles/TileMedium.xaml", UriKind.Absolute);
            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var xmlString = await Windows.Storage.FileIO.ReadTextAsync(file);

            var doc = XDocument.Parse(xmlString);

            XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XNamespace x = "http://schemas.microsoft.com/winfx/2006/xaml";

            var grid = doc.Root?
                    .Descendants(ns + "Grid")
                    .FirstOrDefault(n => n.Attribute(x + "Name")?.Value == "LayoutRoot");

            var control = XamlReader.Load(grid.ToString()) as Grid;

            control.Width = 310;
            control.Height = 310;

            return control;
        }

        private static async Task RenderAndSaveToFileAsync(UIElement element, uint width, uint height, string filename)
        {
            var resultStorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var outputStorageFile = await resultStorageFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            using (var outputFileStream = await outputStorageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await RenderAndSaveToStreamAsync(element, width, height, outputFileStream);
            }
        }

        private static async Task RenderAndSaveToStreamAsync(UIElement element, uint width, uint height, IRandomAccessStream outputFileStream)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(element, (int)width, (int)height);
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var dataReader = DataReader.FromBuffer(pixelBuffer);
            var pixelWidth = (uint)renderTargetBitmap.PixelWidth;
            var pixelHeight = (uint)renderTargetBitmap.PixelHeight;
            // free resource
            renderTargetBitmap = null;

            byte[] buffer = new byte[pixelBuffer.Length];
            dataReader.ReadBytes(buffer);
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, outputFileStream);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, pixelWidth, pixelHeight, 96, 96, buffer);
            await encoder.FlushAsync();

            // free resource
            buffer = null;

            //await outputFileStream.FlushAsync();
        }

        public static IAsyncOperation<WriteableBitmap> RenderMiniToWritableBitmapOperation(UIElement element, uint width, uint height)
        {
            return RenderMiniToWritableBitmap(element, width, height).AsAsyncOperation();
        }

        internal static async Task<WriteableBitmap> RenderMiniToWritableBitmap(UIElement element, uint width, uint height)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(element, (int)width, (int)height);
            var wb = new WriteableBitmap((int)width, (int)height);

            uint pixelWidth = (uint)renderTargetBitmap.PixelWidth;
            uint pixelHeight = (uint)renderTargetBitmap.PixelHeight;
            byte[] pixelBuffer = (await renderTargetBitmap.GetPixelsAsync()).ToArray();

            // free resource
            renderTargetBitmap = null;

            using (var stream = new InMemoryRandomAccessStream())
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, pixelWidth, pixelHeight, 96, 96, pixelBuffer);
                await encoder.FlushAsync();
                stream.Seek(0);
                wb.SetSource(stream);
            }

            // free resource
            pixelBuffer = null;

            // Redraw the WriteableBitmap
            wb.Invalidate();

            return wb;
        }
    }
}