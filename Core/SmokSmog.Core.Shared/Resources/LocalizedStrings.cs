using Windows.ApplicationModel.Resources;

namespace SmokSmog.Resources
{
    public class LocalizedStrings
    {
        private static readonly ResourceLoader _resourceLoader;

        static LocalizedStrings()
        {
            _resourceLoader = new ResourceLoader("SmokSmog.Core/Strings");
        }

        private LocalizedStrings()
        {
        }

        public static string GetString(string key)
        {
            return _resourceLoader.GetString(key);
        }
    }
}