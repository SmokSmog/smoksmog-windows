using System.Threading.Tasks;

namespace SmokSmog.Services.Storage
{
    public interface ISettingService
    {
        /// <summary>
        /// Save setting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SaveSetting<T>(string key, T value);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetSetting<T>(string key);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SaveSettingAsync<T>(string key, T value);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetSettingAsync<T>(string key);
    }
}