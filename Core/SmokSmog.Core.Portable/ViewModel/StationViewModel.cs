namespace SmokSmog.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;
    using Messenger;
    using Model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StationViewModel : ViewModelBase
    {
        private Model.Station _station = null;

        public StationViewModel()
        {
            Messenger.Default.Register<StationChangeMessage>(this, HandleStationChangeMessage);
            PropertyChanged += OnPropertyChanged;
        }

        private void HandleStationChangeMessage(StationChangeMessage message)
        {
            if (message != null && message.Content != null)
            {
                Station = message.Content;
            }
        }

        public AirQualityIndex AirQualityIndex { get; private set; } = AirQualityIndex.Unavaible;

        public bool IsValidStation => Station.Id != -1;

        public List<Model.Parameter> Parameters { get; }

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

        public async Task LoadData(int stationId)
        {
            var dataService = Services.ServiceLocator.Instance.DataService;

            var parameters = (await dataService.GetParametersAsync(stationId)).ToList();
            var measurement = (await dataService.GetMeasurementsAsync(stationId, parameters)).ToList();

            //measurement.First().
            //parameters
        }

        private async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Station))
            {
                await LoadData(Station.Id);
            }
        }
    }
}