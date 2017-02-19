using SmokSmog.Navigation;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Views
{
    [Navigation(ContentType = ContentType.Second)]
    public sealed partial class GeoStationList : Page
    {
        public GeoStationList()
        {
            this.InitializeComponent();
        }
    }
}