/*

Included as link to library implemented interfaces

*/

#if !PORTABLE

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Services.DataService;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services
{
    public class ServiceLocator : IServiceLocator
    {
        static ServiceLocator()
        {
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.Services.DataService>();
                SimpleIoc.Default.Register<IGeolocationService, Design.Services.GeolocationService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, SmokSmogApiDataService>();
                SimpleIoc.Default.Register<IGeolocationService, GeolocationService>();
            }

            SimpleIoc.Default.Register<IFileService, FileService>();
            SimpleIoc.Default.Register<ISettingsService, SettingService>();
        }

        public IFileService FileService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IFileService>();
            }
        }

        public IGeolocationService GeolocationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGeolocationService>();
            }
        }

        public ISettingsService SettingService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISettingsService>();
            }
        }

        public IDataService DataService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IDataService>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}

#endif