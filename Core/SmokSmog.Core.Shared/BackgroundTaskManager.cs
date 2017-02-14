using GalaSoft.MvvmLight;
using SmokSmog.Diagnostics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace SmokSmog
{
    public class BackgroundTaskManager : ObservableObject
    {
        public const string PrimaryTileTimerUpdateBackgroundTaskName = "primaryTileTimerUpdateBackgroundTask";

        public bool IsPrimaryTileTimerUpdateRegidtered
            => PrimaryTileTimerUpdateRegistration != null;

        internal IBackgroundTaskRegistration PrimaryTileTimerUpdateRegistration
            => BackgroundTaskRegistration.AllTasks.FirstOrDefault(
                    x => x.Value.Name == PrimaryTileTimerUpdateBackgroundTaskName).Value;

        /// <summary>
        /// Register Background Tasks for SmokSmog Application
        /// For Windows 8.1 it must be called from UI Thread (best place is MainPage OnNavigatedTo method)
        /// For Windows 10 UWP in documentation we can find Tip that there is no more requirement to run
        /// RequestAccessAsync from UI thread but if we run it from other TimeTriggers will not work
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/uwp/api/Windows.ApplicationModel.Background.BackgroundExecutionManager#Windows_ApplicationModel_Background_BackgroundExecutionManager_RequestAccessAsync_System_String_"/>
        /// <returns></returns>
        public async Task RegisterBackgroundTasks()
        {
            try
            {
                // Without this line sometimes TimeTriggers don't work
                BackgroundExecutionManager.RemoveAccess();

                // Explicit gets the package-relative app identifier (PRAID) for the process.
                var praid = Windows.ApplicationModel.Core.CoreApplication.Id;

                // this line should be run from UI Thread (even for some Windows 10 Devices)
                var status = await BackgroundExecutionManager.RequestAccessAsync(praid);

                // Obsolete in Windows 10 - CS0618
                // but still works and on some Phones with Windows 10 we still get old statuses
#pragma warning disable 618
                switch (status)
                {
                    // Those are for Windows 8.1 and Windows 8.1 Phone
                    case BackgroundAccessStatus.Unspecified:
                    case BackgroundAccessStatus.Denied:
                        return;

                    case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
                    case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                        break;

                    // Those are for Windows 10 Devices
#if WINDOWS_UWP
                    case BackgroundAccessStatus.DeniedBySystemPolicy:
                    case BackgroundAccessStatus.DeniedByUser:
                        return;

                    case BackgroundAccessStatus.AlwaysAllowed:
                    case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                        break;
#endif
                    // For future if something change it should still react properly
                    default: return;
                }

                var registration = PrimaryTileTimerUpdateRegistration;

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
                RaisePropertyChanged(nameof(IsPrimaryTileTimerUpdateRegidtered));
                RaisePropertyChanged(nameof(PrimaryTileTimerUpdateRegistration));
            }
        }

        public void UnregisterTasks()
        {
            PrimaryTileTimerUpdateRegistration?.Unregister(true);
            RaisePropertyChanged(nameof(IsPrimaryTileTimerUpdateRegidtered));
            RaisePropertyChanged(nameof(PrimaryTileTimerUpdateRegistration));
        }
    }
}