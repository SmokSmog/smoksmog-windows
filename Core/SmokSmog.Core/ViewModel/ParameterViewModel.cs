using GalaSoft.MvvmLight;
using SmokSmog.Model;
using SmokSmog.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    public class ParameterViewModel : ViewModelBase
    {
        private AirQualityIndex _airQualityIndex = AirQualityIndex.Unavaible;
        private Measurement _latest = null;
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

        public Measurement Latest => _latest;
        public Measurements Measurements { get; } = new Measurements();

        public Parameter Parameter => _parameter;
        public Station Station => _station;

        public void Clear()
        {
            _airQualityIndex = AirQualityIndex.Unavaible;
            _latest = new Measurement(Station, Parameter);
            Measurements.Clear();

            RaisePropertyChanged(nameof(AirQualityIndex));
            RaisePropertyChanged(nameof(Latest));
            RaisePropertyChanged(nameof(Measurements));
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
                    var querry = measurements.OrderByDescending(o => o.Date).ToList();

                    if (querry.Any())
                    {
                        foreach (var measurement in querry)
                        {
                            Measurements.Add(measurement);
                        }
                        _latest = querry.First();
                        _airQualityIndex = _latest.Aqi;
                    }
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
                RaisePropertyChanged(nameof(Latest));
                RaisePropertyChanged(nameof(Measurements));
            }
        }
    }
}