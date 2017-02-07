using SmokSmog.Controls.Tiles;
using SmokSmog.ViewModel;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Imaging;

namespace SmokSmog.Notification
{
    public sealed class TilesBackgroundTask : XamlRenderingBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely while asynchronous code
            // is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await UpdateTile();

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private static async Task SaveVisualElementToFile(FrameworkElement element, StorageFile file)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(element, (int)element.Width, (int)element.Height);
            var pixels = await renderTargetBitmap.GetPixelsAsync();

            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await
                    BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                byte[] bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Ignore,
                                     (uint)element.Width, (uint)element.Height,
                                     96, 96, bytes);

                await encoder.FlushAsync();
            }
        }

        private static async Task<string> GenerateHighResTileImageUpdateAsync()
        {
            var uri = new Uri("ms-appx:///SmokSmog.Core/Controls/Medium.xml");

            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
                var xmlString = await Windows.Storage.FileIO.ReadTextAsync(file);
                var control = XamlReader.Load(xmlString) as FrameworkElement;

                var vm = new StationViewModel();
                await vm.SetStationAsync(4);

                control.DataContext = vm;
            }
            catch (Exception e)
            {
            }

            // Package.Current.InstalledLocation.GetFolderAsync()

            //Windows.UI.Xaml.Markup.XamlBinaryWriter.Write()

            return string.Empty;
        }

        private static async Task UpdateTile()
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            SecondaryTile st = new SecondaryTile();
            var r = await SecondaryTile.FindAllForPackageAsync();
            foreach (var secondaryTile in r)
            {
                //secondaryTile.Arguments
            }

            await GenerateHighResTileImageUpdateAsync();

            var name = "LargeTile.png";
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var option = Windows.Storage.CreationCollisionOption.ReplaceExisting;

            // create file
            var file = await folder.CreateFileAsync(name, option);

            //AirQualityIndexImage
            //AirQualityIndexValue

            // write content
            //await Windows.Storage.FileIO.WriteBytesAsync(file, null);

            //// acquire file
            //file = await folder.GetFileAsync(name);

            //// read content
            //var _ReadThis = await Windows.Storage.FileIO.ReadTextAsync(file);

            var vm = new StationViewModel();
            await vm.SetStationAsync(4);

            Large large = null;
            if (large != null)
            {
                large.DataContext = vm;
                await SaveVisualElementToFile(large, file);
            }

            // Keep track of the number feed items that get tile notifications.
            //int itemCount = 0;

            //// Create a tile notification for each feed item.
            //foreach (var item in feed.Items)
            //{
            //    XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText03);

            // var title = item.Title; string titleText = title.Text == null ? String.Empty :
            // title.Text; tileXml.GetElementsByTagName(textElementName)[0].InnerText = titleText;

            // // Create a new tile notification. updater.Update(new TileNotification(tileXml));

            //    // Don't create more than 5 notifications.
            //    if (itemCount++ > 5) break;
            //}
        }
    }
}