using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;

namespace SmokSmog.ViewModel
{
    using Services.Data;
    using Services.Storage;

    public class FavoritesViewModel : StationsListBaseViewMode
    {
        private readonly ISettingsService _settingsService;

        private RelayCommand<Model.Station> _addStationToFavoritesCommand;
        private RelayCommand<Model.Station> _removeStationFromFavoritesCommand;
        private RelayCommand<Model.Station> _setHomeStationCommand;

        public FavoritesViewModel(IDataProvider dataService, ISettingsService settingsService)
            : base(dataService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Gets the AddStationToFavoritesCommand.
        /// </summary>
        public RelayCommand<Model.Station> AddStationToFavoritesCommand
        {
            get
            {
                return _addStationToFavoritesCommand
                    ?? (_addStationToFavoritesCommand = new RelayCommand<Model.Station>(
                    p =>
                    {
                        if (p != null) _settingsService.FavoritesStationsList.Add(p.Id);
                        _addStationToFavoritesCommand.RaiseCanExecuteChanged();
                        _removeStationFromFavoritesCommand.RaiseCanExecuteChanged();
                        RaisePropertyChanged(nameof(FavoritesList));
                    },
                    p => p != null && !_settingsService.FavoritesStationsList.Contains(p.Id)));
            }
        }

        public List<Model.Station> FavoritesList
        {
            get
            {
                var list = (from s in StationsList
                            where _settingsService.FavoritesStationsList.Contains(s.Id)
                            select s).ToList();
                IsFavoritesListEmpty = list.Count == 0;
                RaisePropertyChanged(nameof(IsFavoritesListEmpty));
                return list;
            }
        }

        public bool IsFavoritesListEmpty { get; private set; } = true;

        /// <summary>
        /// Gets the RemoveStationFromFavoritesCommand.
        /// </summary>
        public RelayCommand<Model.Station> RemoveStationFromFavoritesCommand
        {
            get
            {
                return _removeStationFromFavoritesCommand
                    ?? (_removeStationFromFavoritesCommand = new RelayCommand<Model.Station>(
                    p =>
                    {
                        if (p != null) _settingsService.FavoritesStationsList.Remove(p.Id);
                        _addStationToFavoritesCommand.RaiseCanExecuteChanged();
                        _removeStationFromFavoritesCommand.RaiseCanExecuteChanged();
                        RaisePropertyChanged(nameof(FavoritesList));
                    },
                    p => p != null && _settingsService.FavoritesStationsList.Contains(p.Id)));
            }
        }

        public Model.Station SampleStation => Model.Station.Sample;

        /// <summary>
        /// Gets the SetHomeStationCommand.
        /// </summary>
        public RelayCommand<Model.Station> SetHomeStationCommand
        {
            get
            {
                return _setHomeStationCommand
                    ?? (_setHomeStationCommand = new RelayCommand<Model.Station>(
                    p =>
                    {
                        if (p != null) _settingsService.HomeStationId = p.Id;
                        _setHomeStationCommand.RaiseCanExecuteChanged();
                    },
                    p => p != null && _settingsService.HomeStationId != p.Id));
            }
        }

        protected override void OnStationListChanged()
        {
            base.OnStationListChanged();
            RaisePropertyChanged(nameof(FavoritesList));
        }
    }
}