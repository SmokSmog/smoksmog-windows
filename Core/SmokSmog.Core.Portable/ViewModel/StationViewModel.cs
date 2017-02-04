namespace SmokSmog.ViewModel
{
    using GalaSoft.MvvmLight;
    using Model;
    using MoreLinq;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum ModelStatus
    {
        Reday,
        Loading,
        Error
    }

    public class ParameterViewModel : ViewModelBase
    {
        private AirQualityIndex _airQualityIndex = AirQualityIndex.Unavaible;

        private Dictionary<AggregationType, Measurement> _lastestMeasurements = new Dictionary
            <AggregationType, Measurement>()
            {
                { AggregationType.Avg1Hour , new Measurement(Station.Empty, null) },
                { AggregationType.Avg8Hour , new Measurement(Station.Empty, null) },
                { AggregationType.Avg24Hour, new Measurement(Station.Empty, null) },
                { AggregationType.Avg1Year , new Measurement(Station.Empty, null) },
            };

        private Dictionary<AggregationType, List<Measurement>> _measurements = new Dictionary
            <AggregationType, List<Measurement>>()
            {
                { AggregationType.Avg1Hour , new List<Measurement>() },
                { AggregationType.Avg8Hour , new List<Measurement>() },
                { AggregationType.Avg24Hour, new List<Measurement>() },
                { AggregationType.Avg1Year , new List<Measurement>() },
            };

        private Parameter _parameter = null;
        private Station _station = Station.Empty;

        public ParameterViewModel(Station station, Parameter parameter)
        {
            _station = station;
            _parameter = parameter;
            Clear();
        }

        /// <summary>
        /// default constructor for design purposes only
        /// </summary>
        internal ParameterViewModel()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
                throw new NotSupportedException();
        }

        public AirQualityIndex AirQualityIndex
        {
            get { return _airQualityIndex; }
            set
            {
                if (_airQualityIndex == value) return;
                _airQualityIndex = value;
                RaisePropertyChanged(nameof(AirQualityIndex));
            }
        }

        public Dictionary<AggregationType, Measurement> LastestMeasurements
        {
            get { return _lastestMeasurements; }
        }

        public Dictionary<AggregationType, List<Measurement>> Measurements
        {
            get { return _measurements; }
        }

        public Parameter Parameter => _parameter;
        public Station Station => _station;

        private AggregationType[] _supportedAggregations =
        {
            AggregationType.Avg1Hour,
            AggregationType.Avg8Hour,
            AggregationType.Avg24Hour,
            AggregationType.Avg1Year,
        };

        public void Clear()
        {
            foreach (var supportedAggregation in _supportedAggregations)
            {
                _measurements[supportedAggregation] = new List<Measurement>();
                _lastestMeasurements[supportedAggregation] = new Measurement(Station, Parameter);
            }
            RaisePropertyChanged(nameof(Measurements));
            RaisePropertyChanged(nameof(LastestMeasurements));
        }

        public async Task LoadData()
        {
            var dataService = ServiceLocatorPortable.Instance.DataService;
            var parameters = (await dataService.GetParametersAsync(Station)).ToList();
            var measurements = (await dataService.GetMeasurementsAsync(Station, new[] { Parameter })).ToList();
        }
    }

    public class StationViewModel : ViewModelBase
    {
        private AirQualityIndex _airQualityIndex = AirQualityIndex.Unavaible;
        private Model.Station _station = null;

        public StationViewModel()
        {
            PropertyChanged += OnPropertyChanged;

            if (IsInDesignMode)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                LoadData(Station);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public AirQualityIndex AirQualityIndex
        {
            get { return _airQualityIndex; }
            private set
            {
                if (_airQualityIndex == value) return;
                _airQualityIndex = value;
                RaisePropertyChanged(nameof(AirQualityIndex));
            }
        }

        public List<Parameter> AqiComponents
        {
            get
            {
                if (Parameters.Any())
                {
                    var list = (from p in Parameters
                                where p.Current.Aqi.Value.HasValue && DateTime.Now - p.Current.Date < TimeSpan.FromMinutes(80)
                                orderby p.Current.Aqi.Value descending
                                select p).ToList();

                    if (list.Any())
                    {
                        var max = list.MaxBy(o => o.Current.Aqi.Value)?.Current;

                        if (max != null)
                        {
                            AirQualityIndex = max.Aqi;
                            return list;
                        }
                    }
                }

                AirQualityIndex = AirQualityIndex.Unavaible;
                return new List<Parameter>();
            }
        }

        public bool IsValidStation => (Station?.Id ?? -1) != -1;

        public List<Parameter> Parameters { get; private set; } = new List<Parameter>();

        public Model.Station Station
        {
            get
            {
                if (IsInDesignModeStatic)
                    return Model.Station.Sample;

                return _station ?? Model.Station.Empty;
            }
            set
            {
                if (_station == value) return;
                _station = value;
                RaisePropertyChanged(nameof(IsValidStation));
                RaisePropertyChanged(nameof(Station));
            }
        }

        public async Task SetStationAsync(int id)
        {
            Station = null;
            try
            {
                var dataService = ServiceLocatorPortable.Instance.DataService;
                var station = await dataService.GetStationAsync(id);
                Station = station;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw;
            }
        }

        private async Task LoadData(Station station)
        {
            Parameters.Clear();
            RaisePropertyChanged(nameof(Parameters));

            try
            {
                var dataService = ServiceLocatorPortable.Instance.DataService;
                var parameters = (await dataService.GetParametersAsync(station)).ToList();
                var measurements = (await dataService.GetMeasurementsAsync(station, parameters)).ToList();

                if (measurements.Any())
                {
                    foreach (var parameter in parameters)
                    {
                        if (parameter != null)
                        {
                            parameter.Measurements =
                                measurements.Where(o => o.Station.Id == station.Id && o.Parameter.Id == parameter.Id)
                                    .ToList();
                        }
                    }

                    Station.Parameters = parameters;
                    Parameters = parameters;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                //throw;
            }

            RaisePropertyChanged(nameof(AqiComponents));
            RaisePropertyChanged(nameof(Parameters));
        }

        private async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Station) && Station.Id > 0)
            {
                await LoadData(Station);
            }
        }
    }
}