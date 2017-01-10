using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmokSmog.Services.Storage
{
    public static class SettingsHelper
    {
        private static IStorageService Storage => ServiceLocator.Instance.SettingService;

        public const string HomeStationIdKey = "HomeStationId";

        public static int? HomeStationId
        {
            get { return Storage.GetSetting<int?>(HomeStationIdKey); }
            set { Storage.SetSetting<int?>(HomeStationIdKey, value); }
        }

        public const string FavoritesStationsListKey = "FavoritesStationsList";

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

        private static void _favoritesStationsList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Storage.SetSetting(FavoritesStationsListKey, _favoritesStationsList.ToList());
        }
    }
}