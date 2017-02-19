using SmokSmog.Services.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SmokSmog.Services.Geolocation
{
    using Model;

    public class GeolocationService : IGeolocationService
    {
        private readonly ISettingsService _settingsService;
        private readonly Geolocator _geolocator;

        //half an kilometer
        private uint? _desiredAccuracyInMeters = 500;

        public uint? DesiredAccuracyInMeters
        {
            get { return _desiredAccuracyInMeters; }
            set
            {
                _desiredAccuracyInMeters = value;
                _geolocator.DesiredAccuracyInMeters = _desiredAccuracyInMeters;
            }
        }

        public GeolocationService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0),
            // DesiredAccuracy.Default is used.
            _geolocator = new Geolocator { DesiredAccuracyInMeters = _desiredAccuracyInMeters };
        }

        public bool LocalizationEnabledInSettings => _settingsService.LocalizationEnable;

        public GeolocationStatus Status
        {
            get
            {
                if (!_settingsService.LocalizationEnable)
                    return GeolocationStatus.Disabled;

                var locationStatus = _geolocator.LocationStatus;
                switch (locationStatus)
                {
                    case PositionStatus.Disabled:
                        return GeolocationStatus.Disabled;

                    case PositionStatus.NotAvailable:
                        return GeolocationStatus.NotAvailable;

                    case PositionStatus.Ready:
                    case PositionStatus.Initializing:
                    case PositionStatus.NoData:
                    case PositionStatus.NotInitialized:
                    default:
                        return GeolocationStatus.Available;
                }
            }
        }
        public Task<Geocoordinate> GetGeocoordinateAsync()
            => GetGeocoordinateAsync(default(CancellationToken));

        public async Task<Geocoordinate> GetGeocoordinateAsync(CancellationToken token)
        {
            try
            {
                // Carry out the operation
                var pos = await _geolocator.GetGeopositionAsync().AsTask(token);

                return new Geocoordinate()
                {
                    Accuracy = pos.Coordinate.Accuracy,
                    Altitude = pos.Coordinate.Point.Position.Altitude,
                    Latitude = pos.Coordinate.Point.Position.Latitude,
                    Longitude = pos.Coordinate.Point.Position.Longitude,
                    Speed = pos.Coordinate.Speed
                };
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // TODO - handle timeout
                // If there are no location sensors GetGeopositionAsync() will timeout -- that is acceptable.
                switch (ex.HResult)
                {
                    case unchecked((int)0x80070102):
                        throw new GeolocationTimeOutException(); // WAIT_TIMEOUT

                    case unchecked((int)0x80004004):
                        // the application does not have the right capability
                        // or the location master switch is off
                        throw new GeolocationDisabledException();

                    case unchecked((int)0x80070005):
                        // E_ACCESSDENIED
                        // An exception of type 'System.UnauthorizedAccessException' occurred in mscorlib.ni.dll but was not handled in user code
                        // Your App does not have permission to access location data.
                        // Make sure you have defined ID_CAP_LOCATION in the application manifest and that on your phone, you have turned on location by checking Settings > Location.
                        // If there is a handler for this exception, the program may be safely continued.

                        throw;
                }
                throw;
            }
        }
    }
}