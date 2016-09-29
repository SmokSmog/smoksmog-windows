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
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

            this.Loaded += page_Loaded;
            this.Unloaded += page_Unloaded;
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

        private void MainPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, GetState(e.NewSize.Width), true);
        }

        private string GetState(double width)
        {
            var w = this;

            if (width <= 440) return "Small";

            return "Default";
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
        }

        public Frame ContentFrame
        {
            get { return ContentFrameXaml; }
        }
    }
}