using System;
using System.Threading.Tasks;
using SmokSmog.Services.Geolocation;

namespace SmokSmog.DesignData.Services
{
    public class GeolocationService : IGeolocationService
    {
        GeolocationStatus IGeolocationService.Status
        {
            get { return GeolocationStatus.Available; }
        }

#pragma warning disable 0067

        public event GeolocationStatusChangeHandler OnStatusChange;

        public bool CancelGetGeocoordinate()
        {
            return true;
        }

        public Task<bool> CancelGetGeocoordinateAsync()
        {
            throw new NotImplementedException();
        }

        public SmokSmog.Model.Geocoordinate GetGeocoordinate()
        {
            return new SmokSmog.Model.Geocoordinate(49.99, 19.91, 500);
        }

#pragma warning disable 1998

        public async Task<SmokSmog.Model.Geocoordinate> GetGeocoordinateAsync()
        {
            return new SmokSmog.Model.Geocoordinate(49.99, 19.91, 500);
        }
    }
}