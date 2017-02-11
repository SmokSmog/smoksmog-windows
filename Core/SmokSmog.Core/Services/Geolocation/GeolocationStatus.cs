namespace SmokSmog.Services.Geolocation
{
    public enum GeolocationStatus
    {
        /// <summary>
        /// Location data is available.
        /// </summary>
        Available,

        /// <summary>
        /// Processing request
        /// </summary>
        Processing,

        /// <summary>
        /// Cacneling request
        /// </summary>
        Canceling,

        /// <summary>
        /// The location provider is disabled. This status indicates that the user has not granted
        /// the application permission to access location.
        /// </summary>
        Disabled,

        /// <summary>
        /// The Windows Sensor and Location Platform is not available on this version of Windows.
        /// </summary>
        NotAvailable
    }
}