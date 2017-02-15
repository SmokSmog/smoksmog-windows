namespace SmokSmog.Services.Storage
{
    public interface IStorageService
    {
        /// <summary>
        /// Save setting
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">  </param>
        /// <param name="value"></param>
        void SetSetting<T>(string key, T value);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetSetting<T>(string key);

        string Language { get; }

        string LanguageCode { get; }
    }
}