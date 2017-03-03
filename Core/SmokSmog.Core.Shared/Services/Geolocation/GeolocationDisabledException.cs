using SmokSmog.Exceptions;

namespace SmokSmog.Services.Geolocation
{
    public class GeolocationDisabledException : ApplicationException
    {
        public GeolocationDisabledException()
            : base("Geolocation disabled in device settings")
        {
        }
    }
}