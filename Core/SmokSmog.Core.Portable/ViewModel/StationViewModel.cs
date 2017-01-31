namespace SmokSmog.ViewModel
{
    using GalaSoft.MvvmLight;
    using Model;
    using MoreLinq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParameterWithMeasurements
    {
        public ParameterWithMeasurements(Parameter parameter, IEnumerable<Measurement> measurements)
        {
            if (parameter == null)
                throw new ArgumentException(nameof(parameter));

            Parameter = parameter;
            Measurements = measurements?.ToList() ?? new List<Measurement>();
        }

        public Measurement LastMeasurement => Measurements.MaxBy(o => o.DateUTC);
        public List<Measurement> Measurements { get; }
        public Parameter Parameter { get; }
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

        public List<ParameterWithMeasurements> AQIComponentsList
        {
            get
            {
                if (ParameterWithMeasurements.Any())
                {
                    var list = from p in ParameterWithMeasurements
                               where p.Measurements.MaxBy(m => m.DateUTC).DateUTC - DateTime.UtcNow < TimeSpan.FromMinutes(80)
                               select p;

                    if (list.Any())
                    {
                        var max = list.Max(p => p.LastMeasurement.DateUTC);
                        var lastMeasurements = from pwm in list where pwm.LastMeasurement.DateUTC - max < TimeSpan.FromMinutes(10) select pwm;
                        if (lastMeasurements.Any())
                        {
                            AirQualityIndex = lastMeasurements.MaxBy(o => o.LastMeasurement.Aqi.Info).LastMeasurement.Aqi;
                            return lastMeasurements.OrderByDescending(o => o.LastMeasurement.Aqi.Value).ToList();
                        }
                    }
                }

                AirQualityIndex = AirQualityIndex.Unavaible;
                return new List<ViewModel.ParameterWithMeasurements>();
            }
        }

        public bool IsValidStation => (Station?.Id ?? -1) != -1;
        public List<ParameterWithMeasurements> ParameterWithMeasurements { get; private set; } = new List<ViewModel.ParameterWithMeasurements>();

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
                var dataService = Services.ServiceLocator.Instance.DataService;
                var station = await dataService.GetStationAsync(id);
                Station = station;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw;
            }
        }

        private async Task LoadData(Model.Station station)
        {
            ParameterWithMeasurements.Clear();
            RaisePropertyChanged(nameof(ParameterWithMeasurements));

            try
            {
                var dataService = Services.ServiceLocator.Instance.DataService;
                var parameters = (await dataService.GetParametersAsync(station)).ToList();
                var measurements = (await dataService.GetMeasurementsAsync(station, parameters)).ToList();

                if (measurements.Any())
                {
                    foreach (var param in parameters)
                    {
                        if (param != null)
                            ParameterWithMeasurements.Add(
                                new ParameterWithMeasurements(param,
                                    (from m in measurements where m.Parameter.Id == param.Id orderby m.DateUTC select m).ToList()));
                    }
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                //throw;
            }

            RaisePropertyChanged(nameof(AQIComponentsList));
            RaisePropertyChanged(nameof(ParameterWithMeasurements));
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