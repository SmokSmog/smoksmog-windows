using SmokSmog.Xaml.UI;

namespace SmokSmog.Views
{
    using Navigation;

    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class DebugPage : StatefulPage
    {
        public DebugPage()
        {
            this.InitializeComponent();
        }

        //public string InstallLocaltion => Package.Current.InstalledLocation.Path;

        //public string LocalFolder => Windows.Storage.ApplicationData.Current.LocalFolder.Path;

        //public async void Load()
        //{
        //    StationViewModel vm = new StationViewModel();
        //    await vm.SetStationAsync(4);
        //    await Task.Delay(1000);

        //    //Large.DataContext = vm;
        //    //WideO.DataContext = vm;
        //    //WideD.DataContext = vm;
        //    //MediumO.DataContext = vm;
        //    //MediumD.DataContext = vm;

        //    //string map = @"SmokSmog.Core/Strings";
        //    //var _resourceLoader = new ResourceLoader(map);
        //    //_resourceLoader.ToString();
        //}
    }
}