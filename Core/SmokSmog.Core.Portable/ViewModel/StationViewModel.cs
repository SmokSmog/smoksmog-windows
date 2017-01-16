namespace SmokSmog.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;
    using Messenger;
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

        public List<Measurement> Measurements { get; }
        public Parameter Parameter { get; }

        public Measurement LastMeasurement => Measurements.MaxBy(o => o.DateUTC);
    }

    public class StationViewModel : ViewModelBase
    {
        private AirQualityIndex _airQualityIndex = AirQualityIndex.Unavaible;
        private Model.Station _station = null;

        public StationViewModel()
        {
            Messenger.Default.Register<StationChangeMessage>(this, HandleStationChangeMessage);
            PropertyChanged += OnPropertyChanged;
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
                if (IsInDesignMode)
                {
                    return new List<ParameterWithMeasurements>()
                    {
                        new ParameterWithMeasurements(
                            new Parameter(1) { Name = "SO2", ShortName="SO2" },
                            new []  {
                                new Measurement(1,1)
                                {
                                    Date = DateTime.Now,
                                    Value = 5.3
                                }
                            }),
                        new ParameterWithMeasurements(
                            new Parameter(1) { Name = "C6H6", ShortName="C6H6" },
                            new []  {
                                new Measurement(1,1)
                                {
                                    Date = DateTime.Now,
                                    Value = 1.3
                                }
                            })
                    };
                }

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
                            AirQualityIndex = lastMeasurements.MaxBy(o => o.LastMeasurement.Aqi.Level).LastMeasurement.Aqi;
                            return lastMeasurements.ToList();
                        }
                    }
                }

                AirQualityIndex = AirQualityIndex.Unavaible;
                return new List<ViewModel.ParameterWithMeasurements>();
            }
        }

        public List<ParameterWithMeasurements> ParameterWithMeasurements { get; private set; } = new List<ViewModel.ParameterWithMeasurements>();

        public bool IsValidStation => Station.Id != -1;

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
                RaisePropertyChanged(nameof(Station));
            }
        }

        public async Task LoadData(Model.Station station)
        {
            var dataService = Services.ServiceLocator.Instance.DataService;

            var parameters = (await dataService.GetParametersAsync(station)).ToList();
            var measurements = (await dataService.GetMeasurementsAsync(station, parameters)).ToList();

            ParameterWithMeasurements.Clear();

            if (measurements.Any())
            {
                foreach (var param in parameters)
                {
                    if (param != null)
                        ParameterWithMeasurements.Add(
                            new ParameterWithMeasurements(param,
                                (from m in measurements where m.ParameterId == param.Id orderby m.DateUTC select m).ToList()));
                }
            }

            RaisePropertyChanged(nameof(AQIComponentsList));
            RaisePropertyChanged(nameof(ParameterWithMeasurements));
        }

        private void HandleStationChangeMessage(StationChangeMessage message)
        {
            if (message != null && message.Content != null)
            {
                Station = message.Content;
            }
        }

        private async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Station))
            {
                await LoadData(Station);
            }
        }
    }
}