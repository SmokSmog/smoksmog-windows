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

            tilesService.PropertyChanged += TilesService_PropertyChanged;
        }

        public bool CanPrimaryTileNotificationEnable
            => _tilesService.CanRegisterBackgroundTasks && _settingsService.HomeStationId.HasValue;

        public bool IsPrimaryTileNotificationEnable
            => _tilesService.IsPrimaryTileNotificationEnable;

        public async Task TooglePrimaryTileNotification()
        {
            var result = _tilesService.IsPrimaryTileNotificationEnable = !IsPrimaryTileNotificationEnable;
            try
            {
                if (CanPrimaryTileNotificationEnable && result)
                    result = await _tilesService.RegisterBackgroundTasks();
                else
                    _tilesService.UnregisterTasks();
            }
            catch (Exception exception)
            {
                Diagnostics.Logger.Log(exception);
            }
            finally
            {
                _tilesService.IsPrimaryTileNotificationEnable = result;
                RaisePropertyChanged(nameof(IsPrimaryTileNotificationEnable));
                RaisePropertyChanged(nameof(CanPrimaryTileNotificationEnable));
            }
        }

        private void TilesService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(IsPrimaryTileNotificationEnable));
            RaisePropertyChanged(nameof(CanPrimaryTileNotificationEnable));
        }
    }
}