using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Net.Http;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;
using Windows.Web.Http;
using ServiceLocation = Microsoft.Practices.ServiceLocation;

namespace SmokSmog.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private static IServiceLocator _current = null;
        private static bool _isInitialized = false;
        private readonly ServiceLocation.IServiceLocator _locator;

        static ServiceLocator()
        {
            Initialize();
        }

        public ServiceLocator()
        {
            Initialize();
            _locator = ServiceLocation.ServiceLocator.Current;
        }

        public static IServiceLocator Current => _current ?? (_current = new ServiceLocator());

        public IDataProvider DataService
            => _locator.GetInstance<IDataProvider>();

        public IGeolocationService GeolocationService
            => _locator.GetInstance<IGeolocationService>();

        public IStorageService SettingService
            => _locator.GetInstance<IStorageService>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }

        public static void Initialize()
        {
            if (_isInitialized) return;

            if (!Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet)
                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (_isInitialized) return;

            SimpleIoc.Default.Register<IHttpClient>(() => new HttpClientProxy(new HttpClient()));

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataProvider, DesignData.Services.ApiDataProvider>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataProvider, SmokSmogApiDataProvider>();
            }

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IGeolocationService, DesignData.Services.GeolocationService>();
            }
            else
            {
                SimpleIoc.Default.Register<IGeolocationService, GeolocationService>();
            }
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            _isInitialized = true;
        }
    }
}