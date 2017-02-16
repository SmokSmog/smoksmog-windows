using SmokSmog.Navigation;
using SmokSmog.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += MainPageLoaded;
            this.Unloaded += MainPageUnloaded;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // here is good place to setup background tasks

            var tileService = Services.ServiceLocator.Current.TilesService;
            await tileService.Initialize();
            base.OnNavigatedTo(e);
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

        public void CloseSearch()
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

        public void OpenSearch()
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
            if (IsSearchOpen) CloseSearch(); else OpenSearch();
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += MainPageSizeChanged;
            SetRootLayout();

            var navProvider = Application.Current as INavigationProvider;
            navProvider?.NavigationService?.NavigateTo(nameof(StationListPage));
            navProvider?.NavigationService?.NavigateTo(nameof(InformationPage));

            navProvider?.NavigationService?.NavigateTo(nameof(NotificationPage));
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
                OpenSearch();
        }

        private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.SearchString))
                CloseSearch();
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