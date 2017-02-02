using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services
{
    internal class ServiceLocatorPortable : IServiceLocator
    {
        private ServiceLocatorPortable()
        {
            if (!Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet)
                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public static IServiceLocator Instance = new ServiceLocatorPortable();

        public IDataProvider DataService => SimpleIoc.Default.GetInstance<IDataProvider>();

        public IGeolocationService GeolocationService => SimpleIoc.Default.GetInstance<IGeolocationService>();

        public IStorageService SettingService => SimpleIoc.Default.GetInstance<IStorageService>();
    }
}