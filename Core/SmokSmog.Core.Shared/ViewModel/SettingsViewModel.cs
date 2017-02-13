using System;
using Windows.ApplicationModel.Background;
using GalaSoft.MvvmLight;

namespace SmokSmog.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public bool PrimaryTileNotificationEnable
        {
            get { return TilesManager.Current.IsPrimaryTileNotificationEnable; }
            set
            {
                TilesManager.Current.IsPrimaryTileNotificationEnable = value;
                RaisePropertyChanged();
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

        public SettingsViewModel()
        {
        }
    }
}