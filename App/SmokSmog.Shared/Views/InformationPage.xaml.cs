using SmokSmog.Navigation;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SmokSmog.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class InformationPage : Page
    {
        public InformationPage()
        {
            this.InitializeComponent();
        }
    }
}