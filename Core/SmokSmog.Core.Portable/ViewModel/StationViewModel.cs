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