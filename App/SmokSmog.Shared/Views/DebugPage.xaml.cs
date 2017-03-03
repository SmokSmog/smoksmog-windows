using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Views
{
    using Navigation;
    using ViewModel;

    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class DebugPage : Page
    {
        public DebugPage()
        {
            this.InitializeComponent();

            Load();
        }

        public string InstallLocaltion => Package.Current.InstalledLocation.Path;

        public string LocalFolder => Windows.Storage.ApplicationData.Current.LocalFolder.Path;

        public async void Load()
        {
            StationViewModel vm = new StationViewModel();
            await vm.SetStationAsync(4);
            await Task.Delay(1000);
            Large.DataContext = vm;
            WideO.DataContext = vm;
            WideD.DataContext = vm;
            MediumO.DataContext = vm;
            MediumD.DataContext = vm;
        }
    }
}