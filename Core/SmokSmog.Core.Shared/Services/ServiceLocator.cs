using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Windows.Web.Http;

namespace SmokSmog.Services
{
    using Data;
    using Geolocation;
    using Network;
    using Notification;
    using Settings;
    using Storage;

    public class ServiceLocator : IServiceLocator
    {
        private static IServiceLocator _current = null;
        private static bool _isInitialized = false;
        private readonly Microsoft.Practices.ServiceLocation.IServiceLocator _locator;

        static ServiceLocator()
        {
            Initialize();
        }

        public ServiceLocator()
        {
            Initialize();
            _locator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current;
        }

        public static IServiceLocator Current => _current ?? (_current = new ServiceLocator());

        public IDataProvider DataService
            => _locator.GetInstance<IDataProvider>();

        public IGeolocationService GeolocationService
            => _locator.GetInstance<IGeolocationService>();

        public IHttpClient HttpClient => _locator.GetInstance<IHttpClient>();

        public ISettingsService SettingsService => _locator.GetInstance<ISettingsService>();

        public IStorageService StorageService => _locator.GetInstance<IStorageService>();

        //#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP #endif
        public ITilesService TilesService => _locator.GetInstance<ITilesService>();

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
            SimpleIoc.Default.Register<IStorageService, StorageService>();
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IGeolocationService, DesignData.Services.GeolocationService>();
                SimpleIoc.Default.Register<IDataProvider, DesignData.Services.ApiDataProvider>();
            }
            else
            {
                SimpleIoc.Default.Register<IGeolocationService, GeolocationService>();
                SimpleIoc.Default.Register<IDataProvider, SmokSmogApiDataProvider>();
            }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            // we need to provide factory method since Tiles Service have internal constructor
            SimpleIoc.Default.Register<ITilesService>(
                () => new TilesService(Current.SettingsService, Current.StorageService));
#endif
            _isInitialized = true;
        }
    }
}