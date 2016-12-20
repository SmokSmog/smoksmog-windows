using SmokSmog.Services.Search;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SmokSmog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ISearchable _searchable = null;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            //ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

            this.Loaded += page_Loaded;
            this.Unloaded += page_Unloaded;
            ContentFrame.Navigated += ContentFrame_Navigated;
        }

        public Frame ContentFrame => ContentFrameXaml;

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
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            // check if ViewModel is search-able
            var page = ContentFrame.Content as Page;
            var viewModel = page?.DataContext;
            _searchable = viewModel as ISearchable;
            SetSearchStatus();
        }

        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var content = ContentFrame.Content;
            var contentString = content?.ToString();
        }

        private string GetState(double width) => width <= 440 ? "Small" : "Default";

        private void MainPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, GetState(e.NewSize.Width), true);
            SetSearchStatus();
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += MainPage_SizeChanged;
            VisualStateManager.GoToState(this, GetState(this.ActualWidth), true);
        }

        private void page_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= MainPage_SizeChanged;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SetSearchStatus(true);
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetSearchStatus();
        }

        private void SetSearchStatus(bool open = false)
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
            VisualStateManager.GoToState(this, GetState(this.ActualWidth), true);
            MenuButtonHamburger.IsChecked = false;
        }

        private void ContentFrameXaml_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, GetState(this.ActualWidth), true);
            MenuButtonHamburger.IsChecked = false;
        }
    }
}