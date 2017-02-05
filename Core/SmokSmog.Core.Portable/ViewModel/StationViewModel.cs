using System.Collections.ObjectModel;

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

    public class ParameterViewModels : ObservableCollection<ParameterViewModel> { }

    public class StationViewModel : ViewModelBase
    {
        private Station _station = null;

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
            get
            {
                if (!AqiComponents.Any()) return AirQualityIndex.Unavaible;

                var max = AqiComponents.MaxBy(o => o.AirQualityIndex.Value);
                return max != null ? max.AirQualityIndex : AirQualityIndex.Unavaible;
            }
        }

        public List<ParameterViewModel> AqiComponents
        {
            get
            {
                Predicate<AirQualityIndex> shouldBeCounted =
                    aqi => aqi.Value.HasValue && DateTime.UtcNow - aqi.DateUtc < TimeSpan.FromMinutes(120);

                if (!Parameters.Any()) return new List<ParameterViewModel>();

                var list = (from p in Parameters
                            where shouldBeCounted(p.AirQualityIndex)
                            orderby p.AirQualityIndex.Value descending
                            select p).ToList();

                if (!list.Any()) return new List<ParameterViewModel>();
                var max = list.MaxBy(o => o.AirQualityIndex.Value);
                return max != null ? list : new List<ParameterViewModel>();
            }
        }

        public bool IsValidStation => (Station?.Id ?? -1) != -1;

        public ParameterViewModels Parameters { get; } = new ParameterViewModels();

        public Station Station
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
                        Parameters.Add(parameterViewModel);
                    }
                    Station.Parameters = parameters;
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                //throw;
            }

            RaisePropertyChanged(nameof(Parameters));
            RaisePropertyChanged(nameof(AqiComponents));
            RaisePropertyChanged(nameof(AirQualityIndex));
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