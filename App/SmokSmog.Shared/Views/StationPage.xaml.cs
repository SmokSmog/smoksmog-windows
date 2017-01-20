using SmokSmog.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog.Views
{
    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class StationPage : Page
    {
        public StationPage()
        {
            this.InitializeComponent();
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
    }
}