using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmokSmog.Services.Settings
{
    using Storage;

    public class SettingsService : ObservableObject, ISettingsService
    {
        public const string FavoritesStationsListKey = "FavoritesStationsList";
        public const string HomeStationIdKey = "HomeStationId";
        public const string LastLaunchedVersionKey = "LastLaunchedVersion";
        public const string LastMainViewKey = "LastMainView";
        public const string LocalizationEnableKey = "LocalizationEnable";
        public const string PrimaryLiveTileEnableKey = "PrimaryLiveTileEnable";

        private readonly IStorageService _storage;
        private ObservableCollection<int> _favoritesStationsList = null;

        public SettingsService(IStorageService storageService)
        {
            _storage = storageService;
        }

        public ObservableCollection<int> FavoritesStationsList
        {
            get
            {
                if (_favoritesStationsList != null)
                    return _favoritesStationsList;

                var list = _storage.GetSetting<List<int>>(FavoritesStationsListKey);
                if (list == null)
                {
                    list = new List<int>();
                    _storage.SetSetting(FavoritesStationsListKey, list);
                }
                _favoritesStationsList = new ObservableCollection<int>(list);
                _favoritesStationsList.CollectionChanged += _favoritesStationsList_CollectionChanged;
                return _favoritesStationsList;
            }
        }

        public int? HomeStationId
        {
            get { return _storage.GetSetting<int?>(HomeStationIdKey); }
            set
            {
                _storage.SetSetting<int?>(HomeStationIdKey, value);
                RaisePropertyChanged();
            }
        }

        public Version LastLaunchedVersion
        {
            get { return _storage.GetSetting<Version>(LastLaunchedVersionKey); }
            set
            {
                _storage.SetSetting<Version>(LastLaunchedVersionKey, value);
                RaisePropertyChanged();
            }
        }

        public string LastMainView
        {
            get { return _storage.GetSetting<string>(LastMainViewKey); }
            set
            {
                _storage.SetSetting<string>(LastMainViewKey, value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// User can enable or disable Localization Services in application
        /// By default it return true (if not specify)
        /// </summary>
        public bool LocalizationEnable
        {
            get { return _storage.GetSetting<bool?>(LocalizationEnableKey) ?? true; }
            set
            {
                _storage.SetSetting<bool?>(LocalizationEnableKey, value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// User can enable or disable primary tile updates
        /// By default it return true (if not specify)
        /// </summary>
        public bool PrimaryLiveTileEnable
        {
            get { return _storage.GetSetting<bool?>(PrimaryLiveTileEnableKey) ?? true; }
            set
            {
                _storage.SetSetting<bool?>(PrimaryLiveTileEnableKey, value);
                RaisePropertyChanged();
            }
        }

        private void _favoritesStationsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _storage.SetSetting(FavoritesStationsListKey, _favoritesStationsList.ToList());
        }
    }
}