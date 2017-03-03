using SmokSmog.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    public enum StationGroupingModeEnum
    {
        Name,
        Province,
        //City,
    }

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.</para>
    /// <para>You can also use Blend to data bind with the tool's support.</para>
    /// <para>See http://www.galasoft.ch/mvvm</para>
    /// </summary>
    public sealed class GroupedViewModel : StationsListBaseViewModel
    {
        // default sort by Name of station
        private StationGroupingModeEnum _currentStationGroupingMode = StationGroupingModeEnum.Name;

        private IList<SmokSmog.Linq.GroupingStation> _stationListGrouped = new List<SmokSmog.Linq.GroupingStation>();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public GroupedViewModel(IDataProvider dataService)
            : base(dataService)
        {
            //if (IsInDesignMode) { /* Code runs in Blend --> create design time data. */ }
        }

        public StationGroupingModeEnum CurrentStationGroupingMode
        {
            get { return _currentStationGroupingMode; }
            set
            {
                if (_currentStationGroupingMode == value) return;
                _currentStationGroupingMode = value;
                RaisePropertyChanged("CurrentStationGroupingMode");
                groupStationListHelper();
            }
        }

        public IList<StationGroupingModeEnum> StationGroupingModeList
        {
            get { return Enum.GetValues(typeof(StationGroupingModeEnum)).Cast<StationGroupingModeEnum>().ToList(); }
        }

        public IList<SmokSmog.Linq.GroupingStation> StationListGrouped
        {
            get { return _stationListGrouped; }
            private set { _stationListGrouped = value; RaisePropertyChanged("StationListGrouped"); }
        }

        protected override void OnStationListChanged()
        {
            base.OnStationListChanged();
            groupStationListHelper();
        }

        private async void groupStationListHelper()
        {
            IList<SmokSmog.Linq.GroupingStation> result = new List<SmokSmog.Linq.GroupingStation>();
            //StationListGrouped = result;

            Func<List<SmokSmog.Linq.GroupingStation>> func = () =>
            {
                #region Grouping Logic

                // to use this text font should be set to Segoe UI Symbol
                List<string> characters = new List<string>()
                {
                    "#","a","ą","b","c","ć","d","e","ę",
                    "f","g","h","i","j","k","l","ł",
                    "m","n","ń","o","ó","p","r","s",
                    "ś","t","u","w","y","z","ź","ż",
                    //'\uE12B'.ToString()
                };

                Dictionary<string, List<Model.Station>> dictionary = new Dictionary<string, List<Model.Station>>();
                switch (CurrentStationGroupingMode)
                {
                    //case StationGroupingModeEnum.City:
                    //    // grouping and sort by city and adress
                    //    foreach (var station in BaseStationsList)
                    //    {
                    //        string key = !string.IsNullOrWhiteSpace(station.City) ? station.City[0].ToString().ToLower() : '\uE12B'.ToString();
                    //        if (!dictionary.ContainsKey(key))
                    //            dictionary[key] = new List<Model.Station>();
                    //        dictionary[key].Add(station);
                    //    }
                    //    foreach (var key in characters)
                    //    {
                    //        if (!dictionary.ContainsKey(key))
                    //            dictionary[key] = new List<Model.Station>();
                    //    }
                    //    return (from pair in dictionary
                    //            orderby pair.Key
                    //            select new SmokSmog.Linq.GroupingStation(
                    //                pair.Key, (from s in pair.Value orderby s.City + s.Address select s))).ToList();

                    case StationGroupingModeEnum.Province:
                        // grouping and sort by province and then by city and adress
                        foreach (var station in StationsList)
                        {
                            string key = !string.IsNullOrWhiteSpace(station.Province) ? station.Province : '\uE12B'.ToString();
                            if (!dictionary.ContainsKey(key))
                                dictionary[key] = new List<Model.Station>();
                            dictionary[key].Add(station);
                        }
                        return (from pair in dictionary
                                orderby pair.Key
                                select new SmokSmog.Linq.GroupingStation(
                                    pair.Key,
                                    from s in pair.Value orderby s.City + s.Address select s)).ToList();

                    case StationGroupingModeEnum.Name:
                    default:
                        // grouping and sort by city and adress
                        foreach (var station in StationsList)
                        {
                            string key = !string.IsNullOrWhiteSpace(station.Name) ? station.Name[0].ToString().ToLower() : "\uE12B";
                            if (!dictionary.ContainsKey(key))
                                dictionary[key] = new List<Model.Station>();
                            dictionary[key].Add(station);
                        }
                        foreach (var key in characters)
                        {
                            if (!dictionary.ContainsKey(key))
                                dictionary[key] = new List<Model.Station>();
                        }
                        return (from pair in dictionary
                                orderby pair.Key
                                select new SmokSmog.Linq.GroupingStation(
                                    pair.Key,
                                    from s in pair.Value orderby s.Name select s)
                                ).ToList();
                }

                #endregion Grouping Logic
            };
            result = await Task.Run(func);

            //for grouping mode selector fade out
            await Task.Delay(TimeSpan.FromMilliseconds(280));
            StationListGrouped = result;
        }
    }
}