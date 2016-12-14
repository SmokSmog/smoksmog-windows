/*

Included as link to library implemented interfaces

*/

#if !PORTABLE

using System.Net.Http;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Net.Http;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;

using ServiceLocation = Microsoft.Practices.ServiceLocation;

namespace SmokSmog.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private ServiceLocation.IServiceLocator _locator;

        static ServiceLocator()
        {
            ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

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

            SimpleIoc.Default.Register<IFileService, FileService>();
            SimpleIoc.Default.Register<ISettingsService, SettingService>();
        }

        public ServiceLocator()
        {
            _locator = ServiceLocation.ServiceLocator.Current;
        }

        public IDataProvider DataService
            => _locator.GetInstance<IDataProvider>();

        public IFileService FileService
                    => _locator.GetInstance<IFileService>();

        public IGeolocationService GeolocationService
            => _locator.GetInstance<IGeolocationService>();

        public ISettingsService SettingService
            => _locator.GetInstance<ISettingsService>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}

#endif