using SmokSmog.Services.Geolocation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SmokSmog.Services.Geolocation
{
    public class GeolocationService : IGeolocationService
    {
        private readonly Geolocator _geolocator;

        private CancellationTokenSource _cts = null;

        //half an kilometer
        private uint? _desiredAccuracyInMeters = 500;

        public event GeolocationStatusChangeHandler OnStatusChange;

        public uint? DesiredAccuracyInMeters
        {
            get { return _desiredAccuracyInMeters; }
            set
            {
                _desiredAccuracyInMeters = value;
                _geolocator.DesiredAccuracyInMeters = _desiredAccuracyInMeters;
            }
        }

        public GeolocationService()
        {
            // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0),
            // DesiredAccuracy.Default is used.
            _geolocator = new Geolocator { DesiredAccuracyInMeters = _desiredAccuracyInMeters };
        }

        public GeolocationStatus Status
        {
            get
            {
                if (_cts != null && IsCancellationRequested)
                {
                    return GeolocationStatus.Canceling;
                }

                // check if actually request is processing
                if (_cts != null)
                {
                    return GeolocationStatus.Processing;
                }

                var locationStatus = new Geolocator().LocationStatus;
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

        private bool IsCancellationRequested = false;

        public bool CancelGetGeocoordinate()
        {
            if (_cts != null)
            {
                IsCancellationRequested = true;
                OnStatusChange?.Invoke(this, null);

                _cts.Cancel(true);
                _cts = null;

                IsCancellationRequested = false;
                return true;
            }
            return false;
        }

        public SmokSmog.Model.Geocoordinate GetGeocoordinate()
        {
            Geoposition pos = _geolocator.GetGeopositionAsync().GetResults();
            return new Model.Geocoordinate()
            {
                Accuracy = pos.Coordinate.Accuracy,
                Altitude = pos.Coordinate.Point.Position.Altitude,
                Latitude = pos.Coordinate.Point.Position.Latitude,
                Longitude = pos.Coordinate.Point.Position.Longitude,
                Speed = pos.Coordinate.Speed
            };
        }

        public async Task<SmokSmog.Model.Geocoordinate> GetGeocoordinateAsync()
        {
            try
            {
                // Get cancellation token
                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                OnStatusChange?.Invoke(this, null);

                // Carry out the operation
                Geoposition pos = await _geolocator.GetGeopositionAsync().AsTask(token);

                return new Model.Geocoordinate()
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
                // If there are no location sensors GetGeopositionAsync() will timeout -- that is acceptable.
                //TODO - handle timeout
                if (ex.HResult == unchecked((int)0x80070102)) // WAIT_TIMEOUT
                {
                    throw new Geolocation.GeolocationTimeOutException();
                }
                else if (ex.HResult == unchecked((int)0x80004004))
                {
                    // the application does not have the right capability or the location master
                    // switch is off
                    throw new System.Exception("Location is disabled in phone settings.");
                }
                else if (ex.HResult == unchecked((int)0x80070005)) //E_ACCESSDENIED
                {
                    throw;
                }

                throw;
            }
            finally
            {
                _cts = null;
                OnStatusChange?.Invoke(this, null);
            }
        }

#pragma warning disable 1998

        public async Task<bool> CancelGetGeocoordinateAsync()
        {
            return CancelGetGeocoordinate();
        }
    }
}