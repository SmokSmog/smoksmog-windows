using GalaSoft.MvvmLight.Command;
using SmokSmog.Extensions;
using SmokSmog.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmokSmog.ViewModel
{
    public class SearchViewModel : StationsListBaseViewMode
    {
        public SearchViewModel(IDataProvider dataService) : base(dataService)
        {
        }

        private List<Model.Station> _stationListFiltered = new List<Model.Station>();
        private string _searchString = string.Empty;

        public RelayCommand ClearFilterCommand => new RelayCommand(() => { SearchString = string.Empty; }, () => true);

        public bool IsStationFilterOn => !string.IsNullOrWhiteSpace(SearchString);

        protected override void OnStationListChanged()
        {
            base.OnStationListChanged();
            RaisePropertyChanged(nameof(StationListFiltered));
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (_searchString == value) return;
                _searchString = value;

                RaisePropertyChanged(nameof(SearchString));
                RaisePropertyChanged(nameof(StationListFiltered));
                RaisePropertyChanged(nameof(IsStationFilterOn));
            }
        }

        public List<Model.Station> StationListFiltered
        {
            get
            {
                Predicate<Model.Station> predicate = (station) => true;

                if (!string.IsNullOrWhiteSpace(SearchString))
                {
                    var expressions = Regex.Replace(SearchString, @"[\s\n]{1,}", " ").Split(' ');

                    predicate = station
                        => $"{station.Name} {station.City} {station.Address} {station.Province}"
                            .ContainsAll(expressions, StringComparison.OrdinalIgnoreCase);
                }

                return (from station in StationsList
                        where predicate(station)
                        orderby station.Name ascending
                        select station).ToList();
            }
        }
    }
}