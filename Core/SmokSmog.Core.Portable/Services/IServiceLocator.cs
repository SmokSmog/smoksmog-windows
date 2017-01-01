using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services
{
    public interface IServiceLocator
    {
        IFileService FileService { get; }

        IGeolocationService GeolocationService { get; }

        ISettingsService SettingService { get; }

        IDataProvider DataService { get; }
    }
}