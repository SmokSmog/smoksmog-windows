using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    using Diagnostics;
    using Model;
    using Resources;
    using Services.Data;
    using Services.Geolocation;

    public class NearestStationListViewModel : StationsListBaseViewModel
    {
        private readonly IGeolocationService _geolocationService;

        private CancellationTokenSource _cancellationTokenSource = null;

        private string _informationLine1 = string.Empty;
        private string _informationLine2 = string.Empty;
        private List<GeoStation> _nearestStations = new List<GeoStation>();

        private States _state = States.Init;

        private string _visualStateString = "Information";

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public NearestStationListViewModel(IDataProvider dataService, IGeolocationService geolocationService)
            : base(dataService)
        {
            _geolocationService = geolocationService;
        }

        public enum States
        {
            Init,
            Localizing,
            Disabled,
            Timeout,
            Cancelled,
            Ready,
            Error
        }

        public RelayCommand CancelGeolocationCommand => new RelayCommand(CancelGetGeolocation, () => true);

        public RelayCommand GetGeolocationCommand => new RelayCommand(GetGeolocationAsync,
            () => _geolocationService.Status == GeolocationStatus.Available);

        public string InformationLine1
        {
            get { return _informationLine1; }
            set
            {
                if (_informationLine1 == value) return;
                _informationLine1 = value;
                RaisePropertyChanged();
            }
        }

        public string InformationLine2
        {
            get { return _informationLine2; }
            set
            {
                if (_informationLine2 == value) return;
                _informationLine2 = value;
                RaisePropertyChanged();
            }
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

        public RelayCommand RefreshGeolocationCommand => new RelayCommand(RefreshGeolocationAsync,
            () => _geolocationService.Status == GeolocationStatus.Available);

        public States State
        {
            get { return _state; }
            set
            {
                if (_state == value) return;
                _state = value;
                RaisePropertyChanged();

                if (value == States.Localizing)
                {
                    VisualStateString = "Localizing";
                    return;
                }

                if (value == States.Ready)
                {
                    VisualStateString = "Ready";
                    return;
                }

                VisualStateString = "Information";
                InformationLine1 = string.Empty;
                InformationLine2 = string.Empty;

                switch (value)
                {
                    case States.Init:
                        InformationLine1 = AppResources.StringInitialization;
                        break;

                    case States.Disabled:
                        InformationLine1 = AppResources.StringLocalizationOff;
                        InformationLine2 = AppResources.StringLocalizationTryAgain;
                        break;

                    case States.Timeout:
                        InformationLine1 = "Timeout";
                        break;

                    case States.Cancelled:
                        InformationLine1 = "Canceled";
                        break;

                    case States.Error:
                        InformationLine1 = "Error";
                        break;
                }
            }
        }

        public string VisualStateString
        {
            get { return _visualStateString; }
            set
            {
                if (_visualStateString == value) return;
                _visualStateString = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="parameters"></param>
        public void Load(object sender, object parameters)
        {
            GetGeolocationAsync();
        }

        private void CancelGetGeolocation()
        {
            _cancellationTokenSource?.Cancel(false);
        }

        private async void GetGeolocationAsync()
        {
            try
            {
                var status = _geolocationService.Status;

                switch (status)
                {
                    case GeolocationStatus.Disabled:
                        State = States.Disabled;
                        return;

                    case GeolocationStatus.NotAvailable:
                        State = States.Disabled;
                        return;
                }

                State = States.Localizing;

                _cancellationTokenSource?.Cancel(false);
                _cancellationTokenSource = new CancellationTokenSource();

                var geocoordinate = await _geolocationService.GetGeocoordinateAsync(_cancellationTokenSource.Token);
                NearestStations = StationsList
                    .Select(station => new GeoStation(station, geocoordinate))
                    .Where(o => o.Distance.HasValue)
                    .OrderBy(o => o.Distance)
                    .Take(10)
                    .ToList();

                State = States.Ready;
            }
            catch (TaskCanceledException)
            {
                State = States.Cancelled;
            }
            catch (UnauthorizedAccessException e)
            {
                Logger.Log(e);
                State = States.Disabled;
            }
            catch (GeolocationTimeOutException e)
            {
                Logger.Log(e);
                State = States.Timeout;
            }
            catch (GeolocationDisabledException e)
            {
                Logger.Log(e);
                State = States.Disabled;
            }
            catch (Exception e)
            {
                Logger.Log(e);
                State = States.Error;
            }
        }

        private void RefreshGeolocationAsync()
        {
            GetGeolocationAsync();
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