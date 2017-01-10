using SmokSmog.Exceptions;

namespace SmokSmog.Services.Geolocation
{
    public class GeolocationTimeOutException : ApplicationException
    {
        public GeolocationTimeOutException() : base("Operation accessing location sensors timed out. Possibly there are no location sensors.")
        {
        }
    }
}