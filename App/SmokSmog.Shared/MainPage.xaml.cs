using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog
{
    using Diagnostics;
    using Navigation;
    using Services;
    using Views;

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += MainPageLoaded;
            this.Unloaded += MainPageUnloaded;
        }

        public bool IsMenuOpen
            => MenuVisualStateGroup.CurrentState == MenuOpen;

        public bool IsSearchOpen { get; private set; }

        private ViewModel.ViewModelLocator ViewModelLocator { get; } = new ViewModel.ViewModelLocator();

        public void CloseMenu()
        {
            if (IsMenuOpen)
                VisualStateManager.GoToState(this, nameof(MenuClose), true);

#if (WINDOWS_APP)
            MenuClose.Storyboard?.Begin();
#endif
        }

        public void CloseSearch(object sender, object parameters)
        {
            SearchTextBox.SearchString = string.Empty;
            IsSearchOpen = false;

            var navProvider = Application.Current as INavigationProvider;
            if (navProvider?.NavigationService?.CurrentSecondPageKey == nameof(SearchPage))
                navProvider?.NavigationService?.NavigateTo(navProvider?.NavigationService.LastSecondPageKey);

            SetSearchLayout();
        }

        public void OpenMenu()
        {
            if (!IsMenuOpen)
                VisualStateManager.GoToState(this, nameof(MenuOpen), true);
#if (WINDOWS_APP)
            MenuOpen.Storyboard?.Begin();
#endif
        }

        public void OpenSearch(object sender, object parameters)
        {
            var navProvider = Application.Current as INavigationProvider;
            if (navProvider?.NavigationService?.CurrentSecondPageKey != nameof(SearchPage))
                navProvider?.NavigationService?.NavigateTo(nameof(SearchPage));

            IsSearchOpen = true;

            SetSearchLayout();
            SearchTextBox.Focus(FocusState.Keyboard);
        }

        public void ToggleMenu()
        {
            if (IsMenuOpen) CloseMenu(); else OpenMenu();
        }

        public void ToggleSearch()
        {
            if (IsSearchOpen)
                CloseSearch(null, null);
            else
                OpenSearch(null, null);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                // here is good place to setup background tasks
                var tileService = ServiceLocator.Current.TilesService;
                await tileService.Initialize();
                await tileService.UpdatePrimaryTile();
            }
            catch (Exception exception)
            {
                Logger.Log(exception);
            }

            // if application run first time since last update (or first install)
            // show short change log
            if (ViewModelLocator.MainViewModel.IsFirstRunAfterUpdate)
            {
                MessageDialog md = new MessageDialog(ViewModelLocator.MainViewModel.Changelog, "Co nowego");
                md.Commands.Add(new UICommand("OK", new UICommandInvokedHandler((cmd) => { })));
                await md.ShowAsync();
            }

            base.OnNavigatedTo(e);
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += MainPageSizeChanged;
            SetRootLayout();

            var navProvider = Application.Current as INavigationProvider;
            navProvider?.NavigationService?.NavigateTo(nameof(StationListPage));

            var homeStationId = ServiceLocator.Current.SettingsService.HomeStationId;

            if (homeStationId.HasValue)
                navProvider?.NavigationService?.NavigateTo(nameof(StationPage), "Home");
            else
                navProvider?.NavigationService?.NavigateTo(nameof(InformationPage));

#if DEBUG
            navProvider?.NavigationService?.NavigateTo(nameof(DebugPage));
#endif
        }

        private void MainPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetRootLayout();
            SetSearchLayout();
        }

        private void MainPageUnloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= MainPageSizeChanged;
        }

        private void SearchTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.SearchString))
                OpenSearch(null, null);
        }

        private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.SearchString))
                CloseSearch(null, null);
        }

        private void SetRootLayout()
        {
            // root layout
            string stateName = "Default";
            if (ActualWidth >= 850) stateName = "Wide";
            if (ActualWidth <= 440) stateName = "Small";
            VisualStateManager.GoToState(this, stateName, true);
        }

        private void SetSearchLayout()
        {
            // search layout
            if (IsSearchOpen)
            {
                if (ActualWidth > 520)
                {
                    VisualStateManager.GoToState(this, "WideSearchState", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "NarrowSearchState", true);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "ClosedSearchState", true);
            }
        }
    }
}