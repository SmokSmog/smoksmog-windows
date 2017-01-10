using SmokSmog.Diagnostics;
using System;
using Windows.Storage;

namespace SmokSmog.Services.Storage
{
    public class StorageService : IStorageService
    {
        public string Language => System.Globalization.CultureInfo.CurrentUICulture.NativeName;

        public string LanguageCode => System.Globalization.CultureInfo.CurrentUICulture.Name;

        public T GetSetting<T>(string key)
        {
            object obj = null;
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out obj))
            {
                try
                {
                    T value = (T)obj;
                    return value;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
            return default(T);
        }

        public void SaveSetting<T>(string key, T value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }
}