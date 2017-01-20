using SmokSmog.Navigation;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Views
{
    [Navigation(ContentType = ContentType.Second)]
    public sealed partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            this.InitializeComponent();
        }
    }
}