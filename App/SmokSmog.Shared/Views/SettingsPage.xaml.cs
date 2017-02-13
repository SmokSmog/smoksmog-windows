using SmokSmog.Navigation;
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
            _backgroundTaskManager = new BackgroundTaskManager();
        }

        private readonly BackgroundTaskManager _backgroundTaskManager;

        private async void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            var @switch = sender as ToggleSwitch;
            if (@switch != null && @switch.IsOn)
                await _backgroundTaskManager.RegisterBackgroundTasks();
            else
                _backgroundTaskManager.UnregisterTasks();
        }
    }
}