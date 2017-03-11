using Windows.ApplicationModel.Resources;

namespace SmokSmog.Resources
{
    public class LocalizedStrings
    {
        private static readonly ResourceLoader _resourceLoader;

        static LocalizedStrings()
        {
            //_resourceLoader = new ResourceLoader("SmokSmog.Core/Strings");
            _resourceLoader = ResourceLoader.GetForViewIndependentUse("SmokSmog.Core/Strings");
        }

        private LocalizedStrings()
        {
        }

        public static string GetString(string key)
        {
            //TODO - notify about missing resources during testing

            string str = _resourceLoader.GetString(key);
            return string.IsNullOrWhiteSpace(str) ? $"({key})" : str;
        }
    }
}