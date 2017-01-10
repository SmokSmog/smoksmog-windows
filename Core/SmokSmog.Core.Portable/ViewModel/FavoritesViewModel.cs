using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmokSmog.Services.Storage;

namespace SmokSmog.ViewModel
{
    public class FavoritesViewModel : ViewModelBase
    {
        private RelayCommand<Model.Station> _setHomeStationCommand;

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
                        if (p != null) SettingsHelper.HomeStationId = p.Id;
                        _setHomeStationCommand.RaiseCanExecuteChanged();
                    },
                    p => p != null && SettingsHelper.HomeStationId != p.Id));
            }
        }

        private RelayCommand<Model.Station> _addStationToFavoritesCommand;

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
                        if (p != null) SettingsHelper.FavoritesStationsList.Add(p.Id);
                        _addStationToFavoritesCommand.RaiseCanExecuteChanged();
                        _removeStationFromFavoritesCommand.RaiseCanExecuteChanged();
                    },
                    p => p != null && !SettingsHelper.FavoritesStationsList.Contains(p.Id)));
            }
        }

        private RelayCommand<Model.Station> _removeStationFromFavoritesCommand;

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
                        if (p != null) SettingsHelper.FavoritesStationsList.Remove(p.Id);
                        _addStationToFavoritesCommand.RaiseCanExecuteChanged();
                        _removeStationFromFavoritesCommand.RaiseCanExecuteChanged();
                    },
                    p => p != null && SettingsHelper.FavoritesStationsList.Contains(p.Id)));
            }
        }
    }
}