using GalaSoft.MvvmLight;
using SmokSmog.Services.Data;
using System.Collections.Generic;
using System.Linq;

namespace SmokSmog.ViewModel
{
    public abstract class StationsListBaseViewModel : ViewModelBase
    {
        protected readonly IDataProvider DataService;

        protected StationsListBaseViewModel(IDataProvider dataService)
        {
            DataService = dataService;
            LoadStationListBase();
        }

        private IList<Model.Station> _stationsList = new List<Model.Station>();

        // changeable only by rest service
        public IList<Model.Station> StationsList
        {
            get { return _stationsList; }
            private set
            {
                _stationsList = value;
                RaisePropertyChanged();
                OnStationListChanged();
            }
        }

        protected virtual void OnStationListChanged()
        {
        }

        private async void LoadStationListBase()
        {
            //TODO make catch and retry 3 times then when it fails return message
            var result = await DataService.GetStationsAsync();
            StationsList = result?.ToList() ?? new List<Model.Station>();
        }
    }
}