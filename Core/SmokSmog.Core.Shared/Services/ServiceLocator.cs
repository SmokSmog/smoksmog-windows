using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Net.Http;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;
using System.Net.Http;
using ServiceLocation = Microsoft.Practices.ServiceLocation;

namespace SmokSmog.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private ServiceLocation.IServiceLocator _locator;

        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (!ServiceLocation.ServiceLocator.IsLocationProviderSet)
                ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (!_isInitialized)
            {
                SimpleIoc.Default.Register<HttpClient>(() => new HttpClient());
                SimpleIoc.Default.Register<IHttpClient, HttpClientProxy>();

                if (ViewModelBase.IsInDesignModeStatic)
                {
                    SimpleIoc.Default.Register<IDataProvider, Design.Services.DesignDataProvider>();
                    SimpleIoc.Default.Register<IGeolocationService, Design.Services.GeolocationService>();
                }
                else
                {
                    SimpleIoc.Default.Register<IDataProvider, SmokSmogApiDataProvider>();
                    SimpleIoc.Default.Register<IGeolocationService, GeolocationService>();
                }

                SimpleIoc.Default.Register<IStorageService, StorageService>();

                _isInitialized = true;
            }
        }

        static ServiceLocator()
        {
            Initialize();
        }

        public ServiceLocator()
        {
            Initialize();
            _locator = ServiceLocation.ServiceLocator.Current;
        }

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
    }
}