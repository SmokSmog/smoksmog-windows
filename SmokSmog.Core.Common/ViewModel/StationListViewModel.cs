using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmokSmog.Extensions;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.RestApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    public enum StationGroupingModeEnum { Name, City, Province }

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.</para>
    /// <para>You can also use Blend to data bind with the tool's support.</para>
    /// <para>See http://www.galasoft.ch/mvvm</para>
    /// </summary>
    public sealed class StationListViewModel : ViewModelBase
    {
        // TODO - implement IDispose !! unregister all events !!

        private IDataService _dataService;
        private IGeolocationService _geolocationService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public StationListViewModel(IGeolocationService geolocationService, IDataService dataService)
        {
            //if (IsInDesignMode) { /* Code runs in Blend --> create design time data. */ }

            _geolocationService = geolocationService;
            if (_geolocationService.Status != GeolocationStatus.NotAvailable)
            {
                _geolocationService.OnStatusChange += GeolocationService_OnStatusChange;
                _getGeolocationButtonVisable = _geolocationService.Status != GeolocationStatus.NotAvailable;
            }

            _dataService = dataService;
            LoadStationListBase();

            PropertyChanged += StationListViewModel_PropertyChanged;
        }

        ~StationListViewModel()
        {
            _geolocationService.OnStatusChange -= GeolocationService_OnStatusChange;
            PropertyChanged -= StationListViewModel_PropertyChanged;
        }

        private void StationListViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        #region Base Station List

        private ObservableCollection<Model.Station> _stationList = new ObservableCollection<Model.Station>();

        private IList<Model.Station> baseStationsList = new List<Model.Station>();

        public ObservableCollection<Model.Station> StationList
        {
            get { return _stationList; }
            set { _stationList = value; RaisePropertyChanged("StationList"); }
        }

        // base changable only by rest service
        private IList<Model.Station> BaseStationsList
        {
            get { return baseStationsList; }
            set
            {
                baseStationsList = value;
                // reseting filter automatically sets public station lists
                StationFilter = string.Empty;
                groupStationListHelper();
            }
        }

        private async void LoadStationListBase()
        {
            BaseStationsList = (await _dataService.GetStationsAsync()).ToList();
        }

        #endregion Base Station List

        #region Geolocation

        private bool _cancelGeolocationButtonVisable = false;

        private string _geocoordinate = "Push the buttn to try to get Geocoordinate";

        private string _geolocationNotyficationText = string.Empty;

        private bool _geolocationNotyficationVisable = false;

        private bool _getGeolocationButtonVisable = false;

        public bool CancelGeolocationButtonVisable
        {
            get { return _cancelGeolocationButtonVisable; }
            set { _cancelGeolocationButtonVisable = value; RaisePropertyChanged("CancelGeolocationButtonVisable"); }
        }

        public RelayCommand CancelGeolocationCommand => new RelayCommand(CancelGetGeolocationAsync, () => true);

        public string Geocoordinate
        {
            get { return _geocoordinate; }
            set { _geocoordinate = value; RaisePropertyChanged("Geocoordinate"); }
        }

        public string GeolocationNotyficationText
        {
            get { return _geolocationNotyficationText; }
            set { _geolocationNotyficationText = value; RaisePropertyChanged("GeolocationNotyficationText"); }
        }

        public bool GeolocationNotyficationVisable
        {
            get { return _geolocationNotyficationVisable; }
            set { _geolocationNotyficationVisable = value; RaisePropertyChanged("GeolocationNotyficationVisable"); }
        }

        public bool GetGeolocationButtonVisable
        {
            get { return _getGeolocationButtonVisable; }
            private set { _getGeolocationButtonVisable = value; RaisePropertyChanged("GetGeolocationButtonVisable"); }
        }

        public RelayCommand GetGeolocationCommand => new RelayCommand(
            GetGeolocationAsync, () => _geolocationService.Status != GeolocationStatus.NotAvailable);

        private async void CancelGetGeolocationAsync()
        {
            CancelGeolocationButtonVisable = false;
            GeolocationNotyficationText = "Anulowanie pobierania lokalizacji ...";
            await _geolocationService.CancelGetGeocoordinateAsync();
        }

        private void GeolocationService_OnStatusChange(IGeolocationService sender, System.EventArgs e)
        {
            CancelGeolocationButtonVisable = false;
            GetGeolocationButtonVisable = false;
            switch (sender.Status)
            {
                case GeolocationStatus.Available:
                    GeolocationNotyficationVisable = false;
                    GetGeolocationButtonVisable = true;
                    break;

                case GeolocationStatus.Processing:
                    GeolocationNotyficationText = "Pobieranie lokalizacji ...";
                    GeolocationNotyficationVisable = true;
                    CancelGeolocationButtonVisable = true;
                    break;

                case GeolocationStatus.Canceling:
                    GeolocationNotyficationText = "Anulowanie pobierania lokalizacji ...";
                    GeolocationNotyficationVisable = true;
                    break;

                case GeolocationStatus.Disabled:
                case GeolocationStatus.NotAvailable:
                    GeolocationNotyficationText = "Usługa lokalizacji jest niedostępna.";
                    GeolocationNotyficationVisable = false;
                    break;

                default:
                    break;
            }
        }

        private async void GetGeolocationAsync()
        {
            //_geolocationService.Status;

            //IsGetGeolocationButtonVisable = false;

            //An exception of type 'System.UnauthorizedAccessException' occurred in mscorlib.ni.dll but was not handled in user code
            //WinRT information: Your App does not have permission to access location data. Make sure you have defined ID_CAP_LOCATION in the application manifest and that on your phone, you have turned on location by checking Settings > Location.
            //Additional information: Odmowa dostępu.
            //Your App does not have permission to access location data. Make sure you have defined ID_CAP_LOCATION in the application manifest and that on your phone, you have turned on location by checking Settings > Location.
            //If there is a handler for this exception, the program may be safely continued.

            Geocoordinate = "Pobieranie pozycji GPS";

            try
            {
                Model.Geocoordinate geocoordinate = await _geolocationService.GetGeocoordinateAsync();
                Geocoordinate = geocoordinate.ToString();
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                Geocoordinate = "Canceled by user";
                //throw;
            }
            catch (System.UnauthorizedAccessException e)
            {
                Geocoordinate = "UnauthorizedAccessException : " + e.Message;
                //throw;
            }
            catch (System.Exception e)
            {
                Geocoordinate = "Exception : " + e.Message;
                //throw;
            }
            //IsGetGeolocationButtonVisable = true;
        }

        #endregion Geolocation

        #region Filtering Station List

        private ObservableCollection<Model.Station> _stationListFiltered = new ObservableCollection<Model.Station>();
        private string _stationsFilter = string.Empty;
        public RelayCommand ClearFilterCommand => new RelayCommand(() => { StationFilter = string.Empty; }, () => true);

        public bool IsStationFilterOn => !string.IsNullOrWhiteSpace(StationFilter);
        public string IsStationFilterOnPropertyName => "IsStationFilterOn";

        public string StationFilter
        {
            get { return _stationsFilter; }
            set
            {
                _stationsFilter = value;
                if (!string.IsNullOrWhiteSpace(_stationsFilter))
                {
                    StationListFiltered.Clear();
                    var expressions = _stationsFilter.Split(' ');

                    foreach (var station in (from station in baseStationsList
                                             where (station.Name + " " + station.City + " " + station.Address + " " + station.Province).ContainsAll(expressions, StringComparison.OrdinalIgnoreCase)
                                             orderby station.Name ascending
                                             select station))
                    {
                        StationListFiltered.Add(station);
                    }
                }
                else
                {
                    StationListFiltered.Clear();
                    foreach (var station in (from station in baseStationsList orderby station.Name ascending select station))
                    {
                        StationListFiltered.Add(station);
                    }
                }

                RaisePropertyChanged(StationFilterPropertyName);
                RaisePropertyChanged(StationListFilteredPropertyName);
                RaisePropertyChanged(IsStationFilterOnPropertyName);
            }
        }

        public string StationFilterPropertyName => "StationFilter";

        public ObservableCollection<Model.Station> StationListFiltered
        {
            get { return _stationListFiltered; }
            set { _stationListFiltered = value; RaisePropertyChanged(StationListFilteredPropertyName); }
        }

        public string StationListFilteredPropertyName => "StationListFiltered";

        #endregion Filtering Station List

        #region Grouping Station List

        // default sort by Name of station
        private StationGroupingModeEnum _currentStationGroupingMode = StationGroupingModeEnum.Name;

        private IList<SmokSmog.Linq.GroupingStation> _stationListGrouped = new List<SmokSmog.Linq.GroupingStation>();

        public StationGroupingModeEnum CurrentStationGroupingMode
        {
            get { return _currentStationGroupingMode; }
            set
            {
                if (_currentStationGroupingMode != value)
                {
                    _currentStationGroupingMode = value;
                    RaisePropertyChanged("CurrentStationGroupingMode");
                    groupStationListHelper();
                }
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

        private async void groupStationListHelper()
        {
            IList<SmokSmog.Linq.GroupingStation> result = new List<SmokSmog.Linq.GroupingStation>();
            StationListGrouped = result;
            Func<List<SmokSmog.Linq.GroupingStation>> func = () =>
            {
                #region Grouping Logic

                // to use this text font should be set to Segoe UI Symbol
                List<string> characters = new List<string>()
                {
                    "#","a","ą","b","c","ć","d","e","ę",
                    "f","g","h","i","j","k","l","ł",
                    "m","n","ń","o","ó","p","r","s",
                    "ś","t","u","w","y","z","ź","ż",'\uE12B'.ToString()
                };

                Dictionary<string, List<Model.Station>> dictionary = new Dictionary<string, List<Model.Station>>();
                switch (CurrentStationGroupingMode)
                {
                    case StationGroupingModeEnum.City:
                        // grouping and sort by city and adress
                        foreach (var station in BaseStationsList)
                        {
                            string key = !string.IsNullOrWhiteSpace(station.City) ? station.City[0].ToString().ToLower() : '\uE12B'.ToString();
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
                                select new SmokSmog.Linq.GroupingStation()
                                {
                                    Key = pair.Key,
                                    Items = (from s in pair.Value orderby s.City + s.Address select s).ToList()
                                }).ToList();

                    case StationGroupingModeEnum.Province:
                        // grouping and sort by province and then by city and adress
                        foreach (var station in BaseStationsList)
                        {
                            string key = !string.IsNullOrWhiteSpace(station.Province) ? station.Province : '\uE12B'.ToString();
                            if (!dictionary.ContainsKey(key))
                                dictionary[key] = new List<Model.Station>();
                            dictionary[key].Add(station);
                        }
                        return (from pair in dictionary
                                orderby pair.Key
                                select new SmokSmog.Linq.GroupingStation()
                                {
                                    Key = pair.Key,
                                    Items = (from s in pair.Value orderby s.City + s.Address select s).ToList()
                                }).ToList();

                    case StationGroupingModeEnum.Name:
                    default:
                        // grouping and sort by city and adress
                        foreach (var station in BaseStationsList)
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
                                select new SmokSmog.Linq.GroupingStation()
                                {
                                    Key = pair.Key,
                                    Items = (from s in pair.Value orderby s.Name select s).ToList()
                                }).ToList();
                }

                #endregion Grouping Logic
            };
            result = await Task.Run(func);
            //for grouping mode selector fade out
            await Task.Delay(TimeSpan.FromMilliseconds(280));
            StationListGrouped = result;
        }

        #endregion Grouping Station List
    }
}