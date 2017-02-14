using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    using Services.Notification;
    using Services.Settings;

    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ITilesService _tilesService;

        public SettingsViewModel(ISettingsService settingsService, ITilesService tilesService)
        {
            _settingsService = settingsService;
            _tilesService = tilesService;
        }

        public bool CanPrimaryTileNotificationEnable
            => _tilesService.CanRegisterBackgroundTasks && _settingsService.HomeStationId.HasValue;

        public bool IsPrimaryTileNotificationEnable
            => _tilesService.IsPrimaryTileNotificationEnable;

        public async Task TooglePrimaryTileNotification()
        {
            bool registered = false;
            try
            {
                if (CanPrimaryTileNotificationEnable)
                    registered = await _tilesService.RegisterBackgroundTasks();
                else
                    _tilesService.UnregisterTasks();
            }
            catch (Exception exception)
            {
                Diagnostics.Logger.Log(exception);
            }
            finally
            {
                _settingsService.PrimaryLiveTileEnable = registered;
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