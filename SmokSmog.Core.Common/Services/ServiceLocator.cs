/*

Included as link to library implemented interfaces

*/

#if !PORTABLE

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Services.Data;
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

        public IDataProvider DataService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IDataProvider>();
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