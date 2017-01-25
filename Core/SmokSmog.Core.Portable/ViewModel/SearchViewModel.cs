using GalaSoft.MvvmLight.Command;
using SmokSmog.Extensions;
using SmokSmog.Model;
using SmokSmog.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.ViewModel
{
    public class SearchViewModel : StationsListBaseViewMode
    {
        private CancellationTokenSource _lastFilterRequestTokenSource = new CancellationTokenSource();
        private Dictionary<Model.Station, string> _searchHashes;
        private string _searchString = string.Empty;

        private List<Model.Station> _stationListFiltered = new List<Station>();

        private readonly Dictionary<string, string> _characterMapping = new Dictionary<string, string>()
        {
            {"ó","o"},
            {"ę","e"},
            {"ą","a"},
            {"ś","s"},
            {"ł","l"},
            {"ż","z"},
            {"ź","z"},
            {"ć","c"},
            {"ń","n"}
        };

        public SearchViewModel(IDataProvider dataService) : base(dataService)
        {
            _searchHashes = GenerateSearchHashes(StationsList);
            PropertyChanged += SearchViewModel_PropertyChanged;
            RaisePropertyChanged(nameof(SearchString));
        }

        public RelayCommand ClearFilterCommand => new RelayCommand(() => { SearchString = string.Empty; }, () => true);

        public bool IsStationFilterOn => !string.IsNullOrWhiteSpace(SearchString);

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (_searchString == value) return;
                _searchString = value;
                RaisePropertyChanged(nameof(SearchString));
            }
        }

        public List<Model.Station> StationListFiltered
        {
            get { return _stationListFiltered; }
            set
            {
                _stationListFiltered = value;
                RaisePropertyChanged(nameof(StationListFiltered));
                RaisePropertyChanged(nameof(IsStationFilterOn));
            }
        }

        public async Task FilterAsync(CancellationToken token)
        {
            await Task.Delay(200, token);

            if (token.IsCancellationRequested)
                return;

            string[] queries = null;

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                var querry = SearchString;
                foreach (var mapping in _characterMapping)
                {
                    querry = querry.Replace(mapping.Key, mapping.Value);

                    if (token.IsCancellationRequested) return;
                }

                queries = Regex.Replace(querry, @"[\s\n]{1,}", " ").Split(' ');
            }

            var list = (from station in StationsList
                        where EvaluateQueries(station, queries)
                        orderby station.Name ascending
                        select station).ToList();

            if (token.IsCancellationRequested)
                return;

            StationListFiltered = list;
        }

        protected override void OnStationListChanged()
        {
            base.OnStationListChanged();
            _searchHashes = GenerateSearchHashes(StationsList);
            RaisePropertyChanged(nameof(SearchString));
        }

        private bool EvaluateQueries(Model.Station station, string[] queries)
        {
            if (queries == null || queries.Length == 0)
                return true;

            return _searchHashes.ContainsKey(station) &&
                   _searchHashes[station].ContainsAll(queries, StringComparison.OrdinalIgnoreCase);
        }

        private Dictionary<Model.Station, string> GenerateSearchHashes(IEnumerable<Model.Station> stationList)
        {
            var list = stationList?.Distinct();
            var dict = new Dictionary<Model.Station, string>();

            if (list == null)
                return dict;

            foreach (var station in list)
            {
                var hash = $"{station.Name} {station.City} {station.Address} {station.Province}";
                foreach (var mapping in _characterMapping)
                {
                    hash = hash.Replace(mapping.Key, mapping.Value);
                }

                dict.Add(station, hash);
            }
            return dict;
        }

        private async void SearchViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchString))
            {
                try
                {
                    _lastFilterRequestTokenSource.Cancel();
                    _lastFilterRequestTokenSource = new CancellationTokenSource();
                    await FilterAsync(_lastFilterRequestTokenSource.Token);
                }
                catch (TaskCanceledException) { }
            }
        }
    }
}