using GalaSoft.MvvmLight;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    using Diagnostics;
    using Model;
    using Services;

    public class ParameterViewModel : ViewModelBase
    {
        public ParameterViewModel(Station station, Parameter parameter)
        {
            Station = station;
            Parameter = parameter;
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

        public AirQualityIndex AirQualityIndex { get; private set; } = AirQualityIndex.Unavaible;

        public Measurement Latest { get; private set; } = null;

        public Measurements Measurements { get; } = new Measurements();

        public Parameter Parameter { get; } = null;

        public Station Station { get; } = Station.Empty;

        public void Clear()
        {
            AirQualityIndex = AirQualityIndex.Unavaible;
            Latest = new Measurement(Station, Parameter);
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
                var dataService = ServiceLocator.Current.DataService;
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
                        Latest = querry.First();
                        AirQualityIndex = Latest.Aqi;
                    }
                }
            }
            catch (Exception exception)
            {
                //TODO - catch all and show notification to user
                Logger.Log(exception);
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