using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Net.Http;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;
using System.Net.Http;

namespace SmokSmog.Services
{
    public class ServiceLocatorPortable : IServiceLocator
    {
        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (!Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet)
                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (_isInitialized) return;

            SimpleIoc.Default.Register<HttpClient>(() => new HttpClient());
            SimpleIoc.Default.Register<IHttpClient, HttpClientProxy>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataProvider, DesignData.Services.ApiDataProvider>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataProvider, SmokSmogApiDataProvider>();
            }

            _isInitialized = true;
        }

        private ServiceLocatorPortable()
        {
            Initialize();
        }

        public static IServiceLocator Instance = new ServiceLocatorPortable();

        public IDataProvider DataService => SimpleIoc.Default.GetInstance<IDataProvider>();

        public IGeolocationService GeolocationService => SimpleIoc.Default.GetInstance<IGeolocationService>();

        public IStorageService SettingService => SimpleIoc.Default.GetInstance<IStorageService>();
    }
}