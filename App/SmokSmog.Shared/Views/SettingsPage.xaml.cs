using SmokSmog.Navigation;
using SmokSmog.ViewModel;
using Windows.UI.Xaml;
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

        private async void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as SettingsViewModel;
            if (vm != null)
            {
                await vm.TooglePrimaryTileNotification();
            }
        }
    }
}