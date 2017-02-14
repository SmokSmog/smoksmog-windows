using GalaSoft.MvvmLight;
using SmokSmog.Services;
using SmokSmog.Services.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmokSmog
{
    public class Settings : ObservableObject
    {
        public const string FavoritesStationsListKey = "FavoritesStationsList";
        public const string HomeStationIdKey = "HomeStationId";
        public const string LastLaunchedVersionKey = "LastLaunchedVersion";
        public const string LastMainViewKey = "LastMainView";
        public const string PrimaryLiveTileEnableKey = "PrimaryLiveTileEnable";
        public const string LocalizationEnableKey = "LocalizationEnable";

        private static Settings _settings = null;
        private ObservableCollection<int> _favoritesStationsList = null;

        private Settings()
        {
        }

        public static Settings Current => _settings ?? (_settings = new Settings());

        public ObservableCollection<int> FavoritesStationsList
        {
            get
            {
                if (_favoritesStationsList != null)
                    return _favoritesStationsList;

                var list = Storage.GetSetting<List<int>>(FavoritesStationsListKey);
                if (list == null)
                {
                    list = new List<int>();
                    Storage.SetSetting(FavoritesStationsListKey, list);
                }
                _favoritesStationsList = new ObservableCollection<int>(list);
                _favoritesStationsList.CollectionChanged += _favoritesStationsList_CollectionChanged;
                return _favoritesStationsList;
            }
        }

        public int? HomeStationId
        {
            get { return Storage.GetSetting<int?>(HomeStationIdKey); }
            set
            {
                Storage.SetSetting<int?>(HomeStationIdKey, value);
                RaisePropertyChanged();
            }
        }

        public Version LastLaunchedVersion
        {
            get { return Storage.GetSetting<Version>(LastLaunchedVersionKey); }
            set
            {
                Storage.SetSetting<Version>(LastLaunchedVersionKey, value);
                RaisePropertyChanged();
            }
        }

        public string LastMainView
        {
            get { return Storage.GetSetting<string>(LastMainViewKey); }
            set
            {
                Storage.SetSetting<string>(LastMainViewKey, value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// User can enable or disable primary tile updates
        /// By default it return true (if not specify)
        /// </summary>
        public bool PrimaryLiveTileEnable
        {
            get { return Storage.GetSetting<bool?>(PrimaryLiveTileEnableKey) ?? true; }
            set
            {
                Storage.SetSetting<bool?>(PrimaryLiveTileEnableKey, value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// User can enable or disable Localization Services in application
        /// By default it return true (if not specify)
        /// </summary>
        public bool LocalizationEnable
        {
            get { return Storage.GetSetting<bool?>(LocalizationEnableKey) ?? true; }
            set
            {
                Storage.SetSetting<bool?>(LocalizationEnableKey, value);
                RaisePropertyChanged();
            }
        }

        private IStorageService Storage => ServiceLocator.Current.SettingService;

        private void _favoritesStationsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Storage.SetSetting(FavoritesStationsListKey, _favoritesStationsList.ToList());
        }
    }
}