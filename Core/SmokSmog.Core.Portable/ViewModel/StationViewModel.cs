
namespace SmokSmog.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public  class StationViewModel : ViewModelBase
    {

        public Model.Station Station
        {
            get { return _station; }
            set
            {
                if (_station == value) return;
                _station = value;
                RaisePropertyChanged(nameof(Station));
            }
        }

        private Model.Station _station = null;


        private RelayCommand _saveAsHomeStation;

        /// <summary>
        /// Gets the SaveAsHome.
        /// </summary>
        public RelayCommand SaveAsHome
        {
            get
            {
                return _saveAsHomeStation
                    ?? (_saveAsHomeStation = new RelayCommand(
                    () =>
                    {

                    },
                    () => true));
            }
        }

    }
}
