using SmokSmog.Porperties;
using SmokSmog.Services.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ISearchable _searchable = null;

        public string ApplicationVersion => AssemblyInfo.Version;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            //ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

            this.Loaded += page_Loaded;
            this.Unloaded += page_Unloaded;
            ContentFrame.Navigated += ContentFrame_Navigated;
            ContentFrame2.SourcePageType = typeof(Views.StationList);
        }

        public Frame ContentFrame => ContentFrame1;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that describes how this page was reached. This parameter is typically used to
        /// configure the page.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are handling the
            // hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event. If you are using the
            // NavigationHelper provided by some templates, this event is handled for you.
            SetMainState();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // check if ViewModel is search-able
            var page = ContentFrame.Content as Page;
            var viewModel = page?.DataContext;
            _searchable = viewModel as ISearchable;
            SetSearchState();
        }

        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var content = ContentFrame.Content;
            var contentString = content?.ToString();
        }

        private void SetMainState()
        {
            double width = ActualWidth;
            string stateName = "Default";
            if (width >= 900) stateName = "Wide";
            if (width <= 440) stateName = "Small";

            VisualStateManager.GoToState(this, stateName, true);
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetMainState();
            SetSearchState();
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += MainPage_SizeChanged;
            SetMainState();
            SetSearchState();
        }

        private void page_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= MainPage_SizeChanged;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SetSearchState(true);
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSearchState();
        }

        private void SetSearchState(bool open = false)
        {
            var stateBefore = SearchVisualStateGroup.CurrentState?.Name;

            if (_searchable == null)
            {
                SearchTextBox.Text = string.Empty;
                VisualStateManager.GoToState(this, "DisableSearchState", true);
                return;
            }

            if ((SearchTextBox.FocusState == FocusState.Unfocused && string.IsNullOrWhiteSpace(SearchTextBox.Text)) && !open)
            {
                VisualStateManager.GoToState(this, "ClosedSearchState", true);
                return;
            }

            if (ActualWidth > 520)
                VisualStateManager.GoToState(this, "WideSearchState", true);
            else
                VisualStateManager.GoToState(this, "NarrowSearchState", true);

            if (stateBefore != "WideSearchState" && stateBefore != "NarrowSearchState")
                SearchTextBox.Focus(FocusState.Keyboard);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_searchable != null)
                _searchable.Querry = new SearchQuerry() { String = SearchTextBox.Text, };
        }

        private void TitleRoot_GotFocus(object sender, RoutedEventArgs e)
        {
            MenuButtonHamburger.IsChecked = false;
        }

        private void ContentFrame1_GotFocus(object sender, RoutedEventArgs e)
        {
            MenuButtonHamburger.IsChecked = false;
        }

        private void HomeButtonClick(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(ContentFrame1, 100);
            Canvas.SetZIndex(ContentFrame2, 0);
            VisualStateManager.GoToState(this, "Frame1", true);
        }

        private void FavoritesButtonClick(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(ContentFrame1, 0);
            Canvas.SetZIndex(ContentFrame2, 100);
            VisualStateManager.GoToState(this, "Frame2", true);
        }
    }
}