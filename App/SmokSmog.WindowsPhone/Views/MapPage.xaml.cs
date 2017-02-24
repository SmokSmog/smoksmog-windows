using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Views
{
    using Navigation;

    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();

            myMap.Loaded += MyMap_Loaded;
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            // center on Notre Dame Cathedral
            var center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = 48.8530,
                    Longitude = 2.3498
                });

            // retrieve map
            myMap.Center = center;
            //await myMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, 3000));
        }
    }
}