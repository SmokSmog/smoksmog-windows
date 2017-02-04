using SmokSmog.Navigation;
using SmokSmog.ViewModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Views
{
    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class NotificationPage : Page
    {
        public NotificationPage()
        {
            this.InitializeComponent();

            Load();
        }

        public async void Load()
        {
            StationViewModel vm = new StationViewModel();
            await vm.SetStationAsync(4);
            await Task.Delay(1000);

            Large.DataContext = vm.Station;
        }
    }
}