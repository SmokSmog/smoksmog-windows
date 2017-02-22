using SmokSmog.Resources;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace SmokSmog.Globalization
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        //static LocalizedStrings()
        //{
        //    _localizedResources = new AppResources();
        //}

        private static AppResources _localizedResources;

        private static ResourceManager _resourceManager;

        public AppResources LocalizedResources => LocalizedResourcesStatic;

        public static AppResources LocalizedResourcesStatic
        {
            get
            {
                if (_localizedResources == null)
                {
                    var currentUiCulture = CultureInfo.CurrentUICulture;
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(currentUiCulture.Name);
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(currentUiCulture.Name);
                    AppResources.Culture = new CultureInfo(CultureInfo.CurrentUICulture.Name);
                    _localizedResources = new AppResources();
                }
                //_localizedResources.ToString();
                return _localizedResources;
            }
        }

        public static string LocalizedString(string resourceKey)
        {
            try
            {
                if (_resourceManager == null)
                {
                    _resourceManager = (ResourceManager)
                        (typeof(SmokSmog.Resources.AppResources).GetRuntimeFields()
                        .First(m => m.Name == "resourceMan").GetValue(null));
                }
                //ResourceManager temp = new ResourceManager("SmokSmog.Resources.AppResources", typeof(LocalizedStrings).GetTypeInfo().Assembly);
                //return _resourceManager.GetString(resourceKey);
                return AppResources.ResourceManager.GetString(resourceKey);
            }
            catch (System.Exception)
            {
                return "LocalizedString not found";
            }
        }
    }
}