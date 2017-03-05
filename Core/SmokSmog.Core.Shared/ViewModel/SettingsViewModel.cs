using GalaSoft.MvvmLight;

namespace SmokSmog.ViewModel
{
    using Services.Notification;
    using Services.Storage;

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
            => _tilesService.CanRegisterBackgroundTasks &&
            _settingsService.HomeStationId.HasValue &&
            _tilesService.IsPrimaryTileTimerUpdateRegidtered;

        public bool IsPrimaryTileNotificationEnable
        {
            get { return _tilesService.IsPrimaryTileNotificationEnable; }
            set
            {
                _tilesService.IsPrimaryTileNotificationEnable = value;
                RaisePropertyChanged();
            }
        }

        private async void TilesService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ITilesService.IsPrimaryTileNotificationEnable):
                    RaisePropertyChanged(nameof(IsPrimaryTileNotificationEnable));
                    await _tilesService.UpdatePrimaryTile();
                    break;

                case nameof(ITilesService.CanRegisterBackgroundTasks):
                    RaisePropertyChanged(nameof(CanPrimaryTileNotificationEnable));
                    break;
            }
        }

        public bool CanLocalizationEnable => true;

        public bool IsLocalizationEnable
        {
            get { return _settingsService.LocalizationEnable; }
            set
            {
                _settingsService.LocalizationEnable = value;
                RaisePropertyChanged();
            }
        }
    }
}