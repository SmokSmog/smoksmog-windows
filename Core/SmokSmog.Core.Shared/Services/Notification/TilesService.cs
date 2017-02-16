﻿using GalaSoft.MvvmLight;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace SmokSmog.Services.Notification
{
    using Diagnostics;
    using Settings;
    using Storage;

    public sealed class TilesService : ObservableObject, ITilesService
    {
        public const string PrimaryTileTimerUpdateBackgroundTaskName = "primaryTileTimerUpdateBackgroundTask";
        private const string PrimaryTileLastUpdateKey = "TilesManager.PrimaryTileLastUpdate";
        private readonly ISettingsService _settingsService;
        private readonly IStorageService _storageService;
        private BackgroundAccessStatus _lastBackgroundAccessStatus = BackgroundAccessStatus.Unspecified;

        internal TilesService(ISettingsService settingsService, IStorageService storageService)
        {
            _settingsService = settingsService;
            _storageService = storageService;
        }

        /// <summary>
        /// For Windows 8.1 call this in OnNavigatedTo form MainPage
        /// </summary>
        /// <returns></returns>
        public bool CanRegisterBackgroundTasks
            => CanRegisterBackgroundTasksFromStatus(_lastBackgroundAccessStatus);

        public bool IsPrimaryTileNotificationEnable
        {
            get { return _settingsService.PrimaryLiveTileEnable; }
            set { _settingsService.PrimaryLiveTileEnable = value; }
        }

        public bool IsPrimaryTileTimerUpdateRegidtered
            => PrimaryTileTimerUpdateRegistration != null;

        // TODO implement secondary tiles
        public bool IsSecondaryTilesNotificationEnable => false;

        public DateTime? PrimaryTileLastUpdate
        {
            get { return _storageService.GetSetting<DateTime?>(PrimaryTileLastUpdateKey); }
            set
            {
                _storageService.SetSetting<DateTime?>(PrimaryTileLastUpdateKey, value);
                RaisePropertyChanged();
            }
        }

        internal IBackgroundTaskRegistration PrimaryTileTimerUpdateRegistration
            => BackgroundTaskRegistration.AllTasks.FirstOrDefault(
                    x => x.Value.Name == PrimaryTileTimerUpdateBackgroundTaskName).Value;

        public async Task Initialize()
        {
            await RegisterBackgroundTasks();
        }

        /// <summary>
        /// Register Background Tasks for SmokSmog Application
        /// For Windows 8.1 it must be called from UI Thread (best place is MainPage OnNavigatedTo method)
        /// For Windows 10 UWP in documentation we can find Tip that there is no more requirement to run
        /// RequestAccessAsync from UI thread but if we run it from other TimeTriggers will not work
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/uwp/api/Windows.ApplicationModel.Background.BackgroundExecutionManager#Windows_ApplicationModel_Background_BackgroundExecutionManager_RequestAccessAsync_System_String_"/>
        /// <returns></returns>
        public async Task<bool> RegisterBackgroundTasks()
        {
            try
            {
                // Explicit gets the package-relative application identifier (PRAID) for the process.
                var praid = Windows.ApplicationModel.Core.CoreApplication.Id;

                // Without this line sometimes TimeTriggers don't work
                BackgroundExecutionManager.RemoveAccess();

                // this line should be run from UI Thread (even for some Windows 10 Devices)
                _lastBackgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync(praid);
                RaisePropertyChanged(nameof(CanRegisterBackgroundTasks));

                var registration = PrimaryTileTimerUpdateRegistration;

                // if background task can not be registered or user disable primary live tile in settings
                if (!CanRegisterBackgroundTasks || !IsPrimaryTileNotificationEnable || IsPrimaryTileTimerUpdateRegidtered)
                {
                    if (registration != null)
                        UnregisterTasks();
                    return false;
                }

                // if there is no registration build new one
                if (registration == null)
                {
                    var builder = new BackgroundTaskBuilder()
                    {
                        Name = PrimaryTileTimerUpdateBackgroundTaskName,
                        //TaskEntryPoint = typeof(SmokSmog.Notification.TilesBackgroundTask).FullName,
                        TaskEntryPoint = "SmokSmog.Notification.TilesBackgroundTask",
#if WINDOWS_UWP || WINDOWS_PHONE
// for Windows 10 and Windows Phone 8.1 we can add Network requirement
                        IsNetworkRequested = true
#endif
                    };

                    IBackgroundCondition condition = new SystemCondition(SystemConditionType.InternetAvailable);
                    builder.AddCondition(condition);

                    // 15 min is the lowest allowed time period
                    IBackgroundTrigger trigger = new TimeTrigger(15, false);
                    //IBackgroundTrigger trigger = new SystemTrigger(SystemTriggerType.TimeZoneChange, false);

                    builder.SetTrigger(trigger);
                    registration = builder.Register();
                }

                return true;

                ////You have the option of implementing these events to do something upon completion
                //registration.Progress += (sender, args) =>
                //{
                //    uint id = args.Progress;
                //};
                //registration.Completed += (sender, args) =>
                //{
                //    args.CheckResult();
                //};
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Debug.WriteLine("The access has already been granted");
            }
            finally
            {
                RaisePropertyChanged(nameof(IsPrimaryTileNotificationEnable));
                RaisePropertyChanged(nameof(IsPrimaryTileTimerUpdateRegidtered));
                RaisePropertyChanged(nameof(CanRegisterBackgroundTasks));
            }
            return false;
        }

        public void UnregisterTasks()
        {
            // Unregister
            PrimaryTileTimerUpdateRegistration?.Unregister(true);

            // if station is invalid clear and return
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

        private bool CanRegisterBackgroundTasksFromStatus(BackgroundAccessStatus status)
        {
            // Obsolete in Windows 10 - CS0618
            // but still works and on some Phones with Windows 10 we still get old statuses
#pragma warning disable 618
            switch (status)
            {
                // Those are for Windows 8.1 and Windows 8.1 Phone
                case BackgroundAccessStatus.Unspecified:
                case BackgroundAccessStatus.Denied:
                    return false;

                case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
                case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                    return true;

                // Those are for Windows 10 Devices
#if WINDOWS_UWP
                case BackgroundAccessStatus.DeniedBySystemPolicy:
                case BackgroundAccessStatus.DeniedByUser:
                    return false;

                case BackgroundAccessStatus.AlwaysAllowed:
                case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                    return true;
#endif
                // For future if something change it should still react properly
                default: return false;
            }
        }
    }
}