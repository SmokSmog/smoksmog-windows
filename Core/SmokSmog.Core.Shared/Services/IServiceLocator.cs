namespace SmokSmog.Services
{
    using Data;
    using Geolocation;
    using Network;
    using Notification;
    using Storage;

    public interface IServiceLocator
    {
        IDataProvider DataService { get; }

        IGeolocationService GeolocationService { get; }

        IHttpClient HttpClient { get; }

        ISettingsService SettingsService { get; }

        IStorageService StorageService { get; }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP

        /// <summary>
        /// This is only for Windows 8.1, Windows Phone 8.1 and Windows 10 UWP
        /// </summary>
        ITilesService TilesService { get; }

#endif
    }
}