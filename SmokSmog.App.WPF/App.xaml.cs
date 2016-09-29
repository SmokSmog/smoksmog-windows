using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace SmokSmog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Services.IServiceLocator ServiceLocator { get; private set; }

        public App()
        {
            //this line is required in order to register all required dependences for injection
            ServiceLocator = new Services.ServiceLocator();
        }

        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}