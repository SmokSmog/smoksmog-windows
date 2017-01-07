﻿using SmokSmog.ViewModel;
using SmokSmog.Xaml.Data.ValueConverters;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SationList : Page
    {
        private StationListViewModel _vm => DataContext as StationListViewModel;

        public SationList()
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

        private void StackPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
                ShowAttachedFlyout(element);
        }

        public void StationListSemanticZoomToggleActiveView()
        {
#if WINDOWS_APP
            StationListSemanticZoom.ToggleActiveView();
#endif
        }

        private void StationListGrouppedListViewTemplate_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
                ShowAttachedFlyout(element);
        }

        private void StationListGrouppedListViewTemplate_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null)
                ShowAttachedFlyout(element);
        }

        private void ShowAttachedFlyout(FrameworkElement element)
        {
            if (element != null)
            {
                var flyout = FlyoutBase.GetAttachedFlyout(element);
                if (flyout != null)
                    FlyoutBase.ShowAttachedFlyout(element);
            }
        }
    }
}