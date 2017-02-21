using GalaSoft.MvvmLight.Command;
using SmokSmog.Diagnostics;
using SmokSmog.Model;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    public class GeoStationListViewModel : StationsListBaseViewModel
    {
        private readonly IGeolocationService _geolocationService;

        private CancellationTokenSource _cancellationTokenSource = null;

        private List<GeoStation> _nearestStations = new List<GeoStation>();

        private string _state = States.Init.ToString();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public GeoStationListViewModel(IDataProvider dataService, IGeolocationService geolocationService)
            : base(dataService)
        {
            _geolocationService = geolocationService;
        }

        public enum States
        {
            Init,
            Localizing,
            Disabled,
            Cancelled,
            Ready,
            Error
        }

        public RelayCommand CancelGeolocationCommand => new RelayCommand(CancelGetGeolocation, () => true);

        public RelayCommand GetGeolocationCommand => new RelayCommand(GetGeolocationAsync,
            () => _geolocationService.Status == GeolocationStatus.Available);

        public RelayCommand RefreshGeolocationCommand => new RelayCommand(RefreshGeolocationAsync,
            () => _geolocationService.Status == GeolocationStatus.Available);

        private void RefreshGeolocationAsync()
        {
            GetGeolocationAsync();
        }

        public void Load(object sender, object parameters)
        {
            Debugger.Break();
            State = States.Localizing.ToString();
        }

        public List<GeoStation> NearestStations
        {
            get { return _nearestStations; }
            set
            {
                if (_nearestStations == value)
                    return;
                _nearestStations = value;
                RaisePropertyChanged();
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                RaisePropertyChanged();
            }
        }

        private void CancelGetGeolocation()
        {
            _cancellationTokenSource?.Cancel(false);
        }

        private async void GetGeolocationAsync()
        {
            //Geocoordinate = "Pobieranie pozycji GPS";

            try
            {
                var status = _geolocationService.Status;

                _cancellationTokenSource?.Cancel(false);
                _cancellationTokenSource = new CancellationTokenSource();

                var geocoordinate = await _geolocationService.GetGeocoordinateAsync(_cancellationTokenSource.Token);
                NearestStations = StationsList
                    .Select(station => new GeoStation(station, geocoordinate))
                    .Where(o => o.Distance.HasValue)
                    .OrderBy(o => o.Distance)
                    .Take(10)
                    .ToList();
            }
            catch (TaskCanceledException)
            {
                //Geocoordinate = "Canceled by user";
            }
            catch (System.UnauthorizedAccessException e)
            {
                Logger.Log(e);
                //Geocoordinate = "UnauthorizedAccessException : " + e.Message;
                //throw;
            }
            catch (GeolocationTimeOutException e)
            {
                Logger.Log(e);
                //Geocoordinate = "Exception : " + e.Message;
                //throw;
            }
            catch (GeolocationDisabledException e)
            {
                Logger.Log(e);
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
            //IsGetGeolocationButtonVisable = true;
        }

        public class GeoStation
        {
            public GeoStation(Station station, Geocoordinate geocoordinate)
            {
                Station = station;
                try
                {
                    Distance = station.Geocoordinate.Distance(geocoordinate, DistanceType);
                }
                catch (Exception e)
                {
                    Logger.Log(e);
                }
            }

            public double? Distance { get; }
            public string DistanceString => Distance.HasValue ? $"{Distance:0.0} km" : "--- km";
            public Station Station { get; }
            private Geocoordinate.DistanceType DistanceType => Geocoordinate.DistanceType.Kilometers;
        }
    }
}