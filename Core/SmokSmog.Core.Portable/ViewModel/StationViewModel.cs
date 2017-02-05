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
        private readonly AggregationType[] _supportedAggregations =
        {
            AggregationType.Avg1Hour,
            AggregationType.Avg8Hour,
            AggregationType.Avg24Hour,
            AggregationType.Avg1Year,
        };

        private AirQualityIndex _airQualityIndex = AirQualityIndex.Unavaible;

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

        public AirQualityIndex AirQualityIndex => _airQualityIndex;

        public Dictionary<AggregationType, Measurement> LastestMeasurements { get; }
            = new Dictionary<AggregationType, Measurement>()
            {
                { AggregationType.Avg1Hour , new Measurement(Station.Empty, null) },
                { AggregationType.Avg8Hour , new Measurement(Station.Empty, null) },
                { AggregationType.Avg24Hour, new Measurement(Station.Empty, null) },
                { AggregationType.Avg1Year , new Measurement(Station.Empty, null) },
            };

        public Dictionary<AggregationType, List<Measurement>> Measurements { get; }
            = new Dictionary<AggregationType, List<Measurement>>()
            {
                { AggregationType.Avg1Hour , new List<Measurement>() },
                { AggregationType.Avg8Hour , new List<Measurement>() },
                { AggregationType.Avg24Hour, new List<Measurement>() },
                { AggregationType.Avg1Year , new List<Measurement>() },
            };

        public Parameter Parameter => _parameter;
        public Station Station => _station;

        public void Clear()
        {
            _airQualityIndex = AirQualityIndex.Unavaible;

            foreach (var supportedAggregation in _supportedAggregations)
            {
                Measurements[supportedAggregation] = new List<Measurement>();
                LastestMeasurements[supportedAggregation] = new Measurement(Station, Parameter);
            }

            RaisePropertyChanged(nameof(AirQualityIndex));
            RaisePropertyChanged(nameof(Measurements));
            RaisePropertyChanged(nameof(LastestMeasurements));
        }

        public async Task LoadData()
        {
            Clear();
            try
            {
                var dataService = ServiceLocatorPortable.Instance.DataService;
                var measurements = (await dataService.GetMeasurementsAsync(Station, new[] { Parameter })).ToList();

                if (measurements.Any())
                {
                    foreach (var aggregation in _supportedAggregations)
                    {
                        var querry = measurements.Where(o => o.Aggregation == aggregation).OrderByDescending(o => o.Date).ToList();
                        if (!querry.Any()) continue;
                        Measurements[aggregation] = querry;
                        LastestMeasurements[aggregation] = querry.First();
                    }

                    _airQualityIndex = LastestMeasurements[AggregationType.Avg1Hour].Aqi;
                }
            }
            catch (Exception exception)
            {
                //TODO - catch all and show notification to user
                SmokSmog.Diagnostics.Logger.Log(exception);
            }
            finally
            {
                RaisePropertyChanged(nameof(AirQualityIndex));
                RaisePropertyChanged(nameof(Measurements));
                RaisePropertyChanged(nameof(LastestMeasurements));
            }
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

        public List<ParameterViewModel> AqiComponents
        {
            get
            {
                Predicate<AirQualityIndex> shouldBeCounted =
                    aqi => aqi.Value.HasValue && DateTime.UtcNow - aqi.DateUtc < TimeSpan.FromMinutes(80);

                if (Parameters.Any())
                {
                    var list = (from p in Parameters
                                where shouldBeCounted(p.AirQualityIndex)
                                orderby p.AirQualityIndex.Value descending
                                select p).ToList();

                    if (list.Any())
                    {
                        var max = list.MaxBy(o => o.AirQualityIndex.Value);

                        if (max != null)
                        {
                            AirQualityIndex = max.AirQualityIndex;
                            return list;
                        }
                    }
                }

                AirQualityIndex = AirQualityIndex.Unavaible;
                return new List<ParameterViewModel>();
            }
        }

        public bool IsValidStation => (Station?.Id ?? -1) != -1;

        public List<ParameterViewModel> Parameters { get; private set; } = new List<ParameterViewModel>();

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

                if (parameters.Any())
                {
                    foreach (var parameter in parameters)
                    {
                        var parameterViewModel = new ParameterViewModel(station, parameter);
                        await parameterViewModel.LoadData();
                    }

                    Station.Parameters = parameters;
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