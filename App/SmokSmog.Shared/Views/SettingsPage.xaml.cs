using SmokSmog.Navigation;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Views
{
    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }
    }
}