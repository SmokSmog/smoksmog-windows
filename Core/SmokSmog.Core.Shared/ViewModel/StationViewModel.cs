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

    public sealed class StationViewModel : ViewModelBase
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
                var result = new List<ParameterViewModel>();

                if (!Parameters.Any())
                    return result;

                var lastest = Parameters.Select(o => o.Latest).ToList();
                if (!lastest.Any())
                    return result;

                var lastDateUtc = lastest.MaxBy(o => o.DateUtc).DateUtc;
                if (DateTime.UtcNow - lastDateUtc > TimeSpan.FromMinutes(80))
                    return result;

                Predicate<AirQualityIndex> shouldBeCounted =
                    aqi => aqi?.Value != null && lastDateUtc - aqi.DateUtc < TimeSpan.FromMinutes(80);

                result = (from p in Parameters
                          where shouldBeCounted(p.AirQualityIndex)
                          orderby p.AirQualityIndex.Value descending
                          select p).ToList();

                return result;
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
                var dataService = ServiceLocator.Current.DataService;
                var station = await dataService.GetStationAsync(id);
                try
                {
                    _loadDataFlag = false;
                    Station = station;
                    await LoadData(station);
                }
                finally
                {
                    _loadDataFlag = true;
                }
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
                var dataService = ServiceLocator.Current.DataService;
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

        private bool _loadDataFlag = true;

        private async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!_loadDataFlag) return;
            if (e.PropertyName == nameof(Station) && Station.Id > 0)
            {
                await LoadData(Station);
            }
        }
    }
}