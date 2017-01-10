using SmokSmog.Services.Data;
using SmokSmog.Services.Geolocation;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services
{
    public interface IServiceLocator
    {
        IGeolocationService GeolocationService { get; }

        IStorageService SettingService { get; }

        IDataProvider DataService { get; }
    }
}