using SmokSmog.ViewModel;
using SmokSmog.Xaml.Data.ValueConverters;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StationList : Page
    {
        private GroupedViewModel _vm => DataContext as GroupedViewModel;

        public StationList()
        {
            this.InitializeComponent();

            var converter = new StationGroupingModeEnumToString();
            foreach (var item in _vm.StationGroupingModeList)
            {
                var menuItem = new MenuFlyoutItem()
                {
                    Text = converter.Convert(item, typeof(string), null, null).ToString(),
                    Command = new GalaSoft.MvvmLight.Command.RelayCommand(()
                        => _vm.CurrentStationGroupingMode = item
                        ),
                };

                menuItem.Tapped += (object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
                    => _vm.CurrentStationGroupingMode = item;

                ChangeGrouppingModeFlyout.Items.Add(menuItem);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that describes how this page was reached. This parameter is typically used to
        /// configure the page.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void StationListSemanticZoomToggleActiveView()
        {
#if WINDOWS_APP
            StationListSemanticZoom.ToggleActiveView();
#endif
        }
    }
}