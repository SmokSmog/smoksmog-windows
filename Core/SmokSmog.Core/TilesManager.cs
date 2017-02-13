using GalaSoft.MvvmLight;
using SmokSmog.Services;
using SmokSmog.Services.Storage;
using System;

namespace SmokSmog
{
    public sealed class TilesManager : ObservableObject
    {
        private static TilesManager _tilesManager;

        private const string PrimaryTileLastUpdateKey = "TilesManager.PrimaryTileLastUpdate";

        private TilesManager()
        {
        }

        public static TilesManager Current => _tilesManager ?? (_tilesManager = new TilesManager());

        public bool IsPrimaryLiveTileEnable
        {
            get
            {
                bool? plte = Settings.Current.PrimaryLiveTileEnable;
                int? homeStationId = Settings.Current.HomeStationId;
                return plte.HasValue && plte.Value && homeStationId.HasValue;
            }
            set
            {
                var homeStationId = Settings.Current.HomeStationId;
                if (!homeStationId.HasValue)
                    return;

                Settings.Current.PrimaryLiveTileEnable = true;
                PrimaryTileLastUpdate = null;
                RaisePropertyChanged();
            }
        }

        public DateTime? PrimaryTileLastUpdate
        {
            get { return Storage.GetSetting<DateTime?>(PrimaryTileLastUpdateKey); }
            set
            {
                Storage.SetSetting<DateTime?>(PrimaryTileLastUpdateKey, value);
                RaisePropertyChanged();
            }
        }

        private IStorageService Storage => ServiceLocator.Current.SettingService;

        // TODO implement secondary tiles
        public bool IsSecondaryLiveTilesEnable => false;
    }
}