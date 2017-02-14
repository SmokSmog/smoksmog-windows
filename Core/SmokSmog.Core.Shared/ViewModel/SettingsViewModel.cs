using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
        }

        public bool CanPrimaryTileNotificationEnable
            => TilesManager.Current.CanRegisterBackgroundTasks && Settings.Current.HomeStationId.HasValue;

        public bool IsPrimaryTileNotificationEnable
            => TilesManager.Current.IsPrimaryTileNotificationEnable;

        public async Task TooglePrimaryTileNotification()
        {
            bool registered = false;
            try
            {
                if (CanPrimaryTileNotificationEnable)
                    registered = await TilesManager.Current.RegisterBackgroundTasks();
                else
                    TilesManager.Current.UnregisterTasks();
            }
            catch (Exception exception)
            {
                Diagnostics.Logger.Log(exception);
            }
            finally
            {
                Settings.Current.PrimaryLiveTileEnable = registered;
                RaisePropertyChanged(nameof(IsPrimaryTileNotificationEnable));
                RaisePropertyChanged(nameof(CanPrimaryTileNotificationEnable));
            }
        }

        //public bool IsBackgroundTasksEnable
        //{
        //    get
        //    {
        //        var status = BackgroundExecutionManager.GetAccessStatus("App");
        //        switch (status)
        //        {
        //            case BackgroundAccessStatus.Unspecified:
        //                break;
        //            case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
        //                break;
        //            case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
        //                break;
        //            case BackgroundAccessStatus.Denied:
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }
        //}
    }
}