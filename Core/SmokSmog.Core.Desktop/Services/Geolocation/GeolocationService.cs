using System;
using System.Threading.Tasks;

namespace SmokSmog.Services.Geolocation
{
    public class GeolocationService : IGeolocationService
    {
        public GeolocationStatus Status
        {
            get { return GeolocationStatus.NotAvailable; }
        }

#pragma warning disable 0067

        public event GeolocationStatusChangeHandler OnStatusChange;

        public bool CancelGetGeocoordinate()
        {
            throw new NotSupportedException();
        }

        public Task<bool> CancelGetGeocoordinateAsync()
        {
            throw new NotImplementedException();
        }

        public SmokSmog.Model.Geocoordinate GetGeocoordinate()
        {
            throw new NotSupportedException();
        }

        public Task<SmokSmog.Model.Geocoordinate> GetGeocoordinateAsync()
        {
            throw new NotSupportedException();
        }
    }
}