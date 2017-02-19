using GalaSoft.MvvmLight.Command;
using SmokSmog.Diagnostics;
using SmokSmog.Model;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    public class GeoStationListViewModel : StationsListBaseViewModel
    {
        private readonly IGeolocationService _geolocationService;

        private CancellationTokenSource _cancellationTokenSource = null;
        private List<Station> _nearestStations = new List<Station>();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public GeoStationListViewModel(IDataProvider dataService, IGeolocationService geolocationService)
            : base(dataService)
        {
            _geolocationService = geolocationService;
        }

        public RelayCommand CancelGeolocationCommand => new RelayCommand(CancelGetGeolocation, () => true);

        public RelayCommand GetGeolocationCommand => new RelayCommand(GetGeolocationAsync,
            () => _geolocationService.Status == GeolocationStatus.Available);

        public List<Station> NearestStations
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
                    .OrderBy(s => s.Geocoordinate.Distance(geocoordinate, Geocoordinate.DistanceType.Meters))
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
    }
}