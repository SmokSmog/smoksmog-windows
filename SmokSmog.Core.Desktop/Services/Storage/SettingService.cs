using System;
using System.Threading.Tasks;

namespace SmokSmog.Services.Storage
{
    public class SettingService : ISettingService
    {
        public T GetSetting<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetSettingAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void SaveSetting<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public Task SaveSettingAsync<T>(string key, T value)
        {
            throw new NotImplementedException();
        }
    }
}