using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;

namespace SmokSmog.ViewModel
{
    public class GeolocationViewModel : ViewModelBase
    {
        private IGeolocationService _geolocationService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public GeolocationViewModel(IDataProvider dataService, IGeolocationService geolocationService)
        {
            //if (IsInDesignMode) { /* Code runs in Blend --> create design time data. */ }
            _geolocationService = geolocationService;
            if (_geolocationService.Status != GeolocationStatus.NotAvailable)
            {
                _geolocationService.OnStatusChange += GeolocationService_OnStatusChange;
                _getGeolocationButtonVisable = _geolocationService.Status != GeolocationStatus.NotAvailable;
            }
        }

        #region Geolocation

        private bool _cancelGeolocationButtonVisable = false;

        private string _geocoordinate = "Push the buttn to try to get Geocoordinate";

        private string _geolocationNotyficationText = string.Empty;

        private bool _geolocationNotyficationVisable = false;

        private bool _getGeolocationButtonVisable = false;

        public bool CancelGeolocationButtonVisable
        {
            get { return _cancelGeolocationButtonVisable; }
            set { _cancelGeolocationButtonVisable = value; RaisePropertyChanged("CancelGeolocationButtonVisable"); }
        }

        public RelayCommand CancelGeolocationCommand => new RelayCommand(CancelGetGeolocationAsync, () => true);

        public string Geocoordinate
        {
            get { return _geocoordinate; }
            set { _geocoordinate = value; RaisePropertyChanged("Geocoordinate"); }
        }

        public string GeolocationNotyficationText
        {
            get { return _geolocationNotyficationText; }
            set { _geolocationNotyficationText = value; RaisePropertyChanged("GeolocationNotyficationText"); }
        }

        public bool GeolocationNotyficationVisable
        {
            get { return _geolocationNotyficationVisable; }
            set { _geolocationNotyficationVisable = value; RaisePropertyChanged("GeolocationNotyficationVisable"); }
        }

        public bool GetGeolocationButtonVisable
        {
            get { return _getGeolocationButtonVisable; }
            private set { _getGeolocationButtonVisable = value; RaisePropertyChanged("GetGeolocationButtonVisable"); }
        }

        public RelayCommand GetGeolocationCommand => new RelayCommand(
            GetGeolocationAsync, () => _geolocationService.Status != GeolocationStatus.NotAvailable);

        private async void CancelGetGeolocationAsync()
        {
            CancelGeolocationButtonVisable = false;
            GeolocationNotyficationText = "Anulowanie pobierania lokalizacji ...";
            await _geolocationService.CancelGetGeocoordinateAsync();
        }

        private void GeolocationService_OnStatusChange(IGeolocationService sender, System.EventArgs e)
        {
            CancelGeolocationButtonVisable = false;
            GetGeolocationButtonVisable = false;
            switch (sender.Status)
            {
                case GeolocationStatus.Available:
                    GeolocationNotyficationVisable = false;
                    GetGeolocationButtonVisable = true;
                    break;

                case GeolocationStatus.Processing:
                    GeolocationNotyficationText = "Pobieranie lokalizacji ...";
                    GeolocationNotyficationVisable = true;
                    CancelGeolocationButtonVisable = true;
                    break;

                case GeolocationStatus.Canceling:
                    GeolocationNotyficationText = "Anulowanie pobierania lokalizacji ...";
                    GeolocationNotyficationVisable = true;
                    break;

                case GeolocationStatus.Disabled:
                case GeolocationStatus.NotAvailable:
                    GeolocationNotyficationText = "Usługa lokalizacji jest niedostępna.";
                    GeolocationNotyficationVisable = false;
                    break;

                default:
                    break;
            }
        }

        private async void GetGeolocationAsync()
        {
            //_geolocationService.Status;

            //IsGetGeolocationButtonVisable = false;

            //An exception of type 'System.UnauthorizedAccessException' occurred in mscorlib.ni.dll but was not handled in user code
            //WinRT information: Your App does not have permission to access location data. Make sure you have defined ID_CAP_LOCATION in the application manifest and that on your phone, you have turned on location by checking Settings > Location.
            //Additional information: Odmowa dostępu.
            //Your App does not have permission to access location data. Make sure you have defined ID_CAP_LOCATION in the application manifest and that on your phone, you have turned on location by checking Settings > Location.
            //If there is a handler for this exception, the program may be safely continued.

            Geocoordinate = "Pobieranie pozycji GPS";

            try
            {
                Model.Geocoordinate geocoordinate = await _geolocationService.GetGeocoordinateAsync();
                Geocoordinate = geocoordinate.ToString();
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                Geocoordinate = "Canceled by user";
                //throw;
            }
            catch (System.UnauthorizedAccessException e)
            {
                Geocoordinate = "UnauthorizedAccessException : " + e.Message;
                //throw;
            }
            catch (System.Exception e)
            {
                Geocoordinate = "Exception : " + e.Message;
                //throw;
            }
            //IsGetGeolocationButtonVisable = true;
        }

        #endregion Geolocation
    }
}