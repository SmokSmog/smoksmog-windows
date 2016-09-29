﻿/*

Included as link to librarys implemented interfaces

*/

#if !PORTABLE

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.RestApi;
using SmokSmog.Services.Storage;
using System;

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
                SimpleIoc.Default.Register<IDataService, DataService>();
                SimpleIoc.Default.Register<IGeolocationService, GeolocationService>();
            }

            SimpleIoc.Default.Register<IFileService, FileService>();
            SimpleIoc.Default.Register<ISettingService, SettingService>();
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

        public ISettingService SettingService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISettingService>();
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