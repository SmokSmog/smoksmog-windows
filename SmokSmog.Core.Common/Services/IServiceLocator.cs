using SmokSmog.Services.Geolocation;
using SmokSmog.Services.RestApi;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services
{
    public interface IServiceLocator
    {
        IFileService FileService { get; }

        IGeolocationService GeolocationService { get; }

        ISettingsService SettingService { get; }

        IDataService DataService { get; }
    }
}