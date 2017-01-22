using SmokSmog.Services;
using SmokSmog.Services.Storage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmokSmog
{
    public static class Settings
    {
        public const string FavoritesStationsListKey = "FavoritesStationsList";
        public const string HomeStationIdKey = "HomeStationId";
        public const string LastMainViewKey = "LastMainView";

        private static ObservableCollection<int> _favoritesStationsList = null;

        public static ObservableCollection<int> FavoritesStationsList
        {
            get
            {
                if (_favoritesStationsList == null)
                {
                    var list = Storage.GetSetting<List<int>>(FavoritesStationsListKey);
                    if (list == null)
                    {
                        list = new List<int>();
                        Storage.SetSetting(FavoritesStationsListKey, list);
                    }
                    _favoritesStationsList = new ObservableCollection<int>(list);
                    _favoritesStationsList.CollectionChanged += _favoritesStationsList_CollectionChanged;
                }
                return _favoritesStationsList;
            }
        }

        public static int? HomeStationId
        {
            get { return Storage.GetSetting<int?>(HomeStationIdKey); }
            set { Storage.SetSetting<int?>(HomeStationIdKey, value); }
        }

        public static string LastMainView
        {
            get { return Storage.GetSetting<string>(LastMainViewKey); }
            set { Storage.SetSetting<string>(LastMainViewKey, value); }
        }

        private static IStorageService Storage => ServiceLocator.Instance.SettingService;

        private static void _favoritesStationsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Storage.SetSetting(FavoritesStationsListKey, _favoritesStationsList.ToList());
        }
    }
}