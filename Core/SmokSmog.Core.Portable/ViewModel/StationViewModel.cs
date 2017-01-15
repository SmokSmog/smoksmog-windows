namespace SmokSmog.ViewModel
{
    using GalaSoft.MvvmLight;
    using Model;

    public class StationViewModel : ViewModelBase
    {
        private Model.Station _station = null;
        public bool IsValidStation => Station.Id != -1;

        public AirQualityIndex AirQualityIndex { get; private set; } = AirQualityIndex.Unavaible;

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
    }
}