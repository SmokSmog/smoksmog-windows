using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.DesignData.Services
{
    using Model;
    using SmokSmog.Services.Geolocation;

    public class GeolocationService : IGeolocationService
    {
        GeolocationStatus IGeolocationService.Status
            => GeolocationStatus.Available;

#pragma warning disable 1998

        public async Task<Geocoordinate> GetGeocoordinateAsync()
        {
            return new Geocoordinate(49.99, 19.91, 500);
        }

#pragma warning disable 1998

        public async Task<Geocoordinate> GetGeocoordinateAsync(CancellationToken token)
        {
            return new Geocoordinate(49.99, 19.91, 500);
        }
    }
}