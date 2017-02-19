using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Services.Geolocation
{
    using Model;

    /// <summary>
    /// </summary>
    public interface IGeolocationService
    {
        /// <summary>
        /// </summary>
        GeolocationStatus Status { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<Geocoordinate> GetGeocoordinateAsync();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<Geocoordinate> GetGeocoordinateAsync(CancellationToken token);
    }
}