using System;
using System.Threading.Tasks;

namespace SmokSmog.Services.Geolocation
{
    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void GeolocationStatusChangeHandler(IGeolocationService sender, EventArgs e);

    /// <summary>
    /// </summary>
    public interface IGeolocationService
    {
        /// <summary>
        /// </summary>
        GeolocationStatus Status { get; }

        /// <summary>
        /// </summary>
        event GeolocationStatusChangeHandler OnStatusChange;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        bool CancelGetGeocoordinate();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<bool> CancelGetGeocoordinateAsync();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Model.Geocoordinate GetGeocoordinate();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<Model.Geocoordinate> GetGeocoordinateAsync();
    }
}