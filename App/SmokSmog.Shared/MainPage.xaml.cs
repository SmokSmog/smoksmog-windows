using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog
{
    public sealed partial class MainPage : Page
    {
        private ViewModel.ViewModelLocator ViewModelLocator { get; } = new ViewModel.ViewModelLocator();

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += page_Loaded;
            this.Unloaded += page_Unloaded;

            SecondFrame.SourcePageType = typeof(Views.StationListPage);
            MainFrame.SourcePageType = typeof(Views.StationPage);

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
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are handling the
            // hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event. If you are using the
            // NavigationHelper provided by some templates, this event is handled for you.
            SetLayoutVisualState();
        }

        //private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        //{
        //    var content = ContentFrame.Content;
        //    var contentString = content?.ToString();
        //}

        private void SetLayoutVisualState()
        {
            double width = ActualWidth;
            string stateName = "Default";
            if (width >= 850) stateName = "Wide";
            if (width <= 440) stateName = "Small";

            VisualStateManager.GoToState(this, stateName, true);
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetLayoutVisualState();
            SetSearchState();
        }

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += MainPage_SizeChanged;
            SetLayoutVisualState();
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

            //if (_searchable == null)
            //{
            //    SearchTextBox.Text = string.Empty;
            //    VisualStateManager.GoToState(this, "DisableSearchState", true);
            //    return;
            //}

            if ((SearchTextBox.FocusState == FocusState.Unfocused && string.IsNullOrWhiteSpace(SearchTextBox.Text)) && !open)
            {
                VisualStateManager.GoToState(this, "ClosedSearchState", true);
                //if (!ContentFrame2.CurrentSourcePageType.Equals(typeof(Views.SearchPage)) && ContentFrame2.CanGoBack)
                //    ContentFrame2.GoBack();
                return;
            }

            //if (!ContentFrame2.CurrentSourcePageType.Equals(typeof(Views.SearchPage)))
            //    ContentFrame2.Navigate(typeof(Views.SearchPage));

            if (ActualWidth > 520)
                VisualStateManager.GoToState(this, "WideSearchState", true);
            else
                VisualStateManager.GoToState(this, "NarrowSearchState", true);

            if (stateBefore != "WideSearchState" && stateBefore != "NarrowSearchState")
                SearchTextBox.Focus(FocusState.Keyboard);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchString = SearchTextBox.Text;
            ViewModelLocator.SearchViewModel.SearchString = searchString;

            if (string.IsNullOrWhiteSpace(searchString))
            {
                if (!SecondFrame.CurrentSourcePageType.Equals(typeof(Views.SearchPage)) && SecondFrame.CanGoBack)
                    SecondFrame.GoBack();
            }
            else
            {
                if (!SecondFrame.CurrentSourcePageType.Equals(typeof(Views.SearchPage)))
                    SecondFrame.Navigate(typeof(Views.SearchPage));
            }
        }

        private void HomeButtonClick(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(MainFrame, 100);
            Canvas.SetZIndex(SecondFrame, 0);
            VisualStateManager.GoToState(this, "Frame1", true);
        }

        private void FavoritesButtonClick(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(MainFrame, 0);
            Canvas.SetZIndex(SecondFrame, 100);
            VisualStateManager.GoToState(this, "Frame2", true);
        }

        public void MenuCloseHelper(object sender, object parameters)
        {
#if (WINDOWS_APP)
            MenuClose.Storyboard?.Begin();
#endif
        }
    }
}