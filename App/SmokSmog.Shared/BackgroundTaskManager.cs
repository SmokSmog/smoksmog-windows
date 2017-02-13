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

        public IBackgroundTaskRegistration PrimaryTileTimerUpdateRegistration
            => BackgroundTaskRegistration.AllTasks.FirstOrDefault(
                    x => x.Value.Name == PrimaryTileTimerUpdateBackgroundTaskName).Value;

        /// <summary>
        /// Register Background Tasks for SmokSmog Application
        /// For Windows 8.1 it must be called from UI Thread (best place is MainPage OnNavigatedTo method)
        /// For Windows 10 UWP in documentation we can find Tip that there is no more requirement to run
        /// RequestAccessAsync from UI thread but if we run it from other TimeTriggers will not work
        /// </summary>
        /// <returns></returns>
        public async Task RegisterBackgroundTasks()
        {
            try
            {
                BackgroundExecutionManager.RemoveAccess();
                var praid = Windows.ApplicationModel.Core.CoreApplication.Id;
                var status = await BackgroundExecutionManager.RequestAccessAsync(praid);

                switch (status)
                {
                    case BackgroundAccessStatus.Unspecified:
                    case BackgroundAccessStatus.Denied:
                        return;

                    case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
                    case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                        break;

#if WINDOWS_UWP
                    case BackgroundAccessStatus.DeniedBySystemPolicy:
                    case BackgroundAccessStatus.DeniedByUser:
                        return;

                    case BackgroundAccessStatus.AlwaysAllowed:
                    case BackgroundAccessStatus.AllowedSubjectToSystemPolicy:
                        break;
#endif
                    default: return;
                }

                var registration = PrimaryTileTimerUpdateRegistration;

                // if there is no registration build new
                if (registration == null)
                {
                    var builder = new BackgroundTaskBuilder()
                    {
                        Name = PrimaryTileTimerUpdateBackgroundTaskName,
                        TaskEntryPoint = typeof(SmokSmog.Notification.TilesBackgroundTask).FullName,
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