//using SmokSmog.Resources;
//using System.Globalization;

//namespace SmokSmog.Globalization
//{
//    /// <summary>
//    /// Provides access to string resources.
//    /// </summary>
//    public class LocalizedStrings
//    {
//        private static AppResources _localizedResources;

//        public AppResources LocalizedResources => LocalizedResourcesStatic;

//        public static AppResources LocalizedResourcesStatic
//        {
//            get
//            {
//                if (_localizedResources != null)
//                    return _localizedResources;

//                var currentUiCulture = CultureInfo.CurrentUICulture;
//                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(currentUiCulture.Name);
//                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(currentUiCulture.Name);
//                AppResources.Culture = new CultureInfo(CultureInfo.CurrentUICulture.Name);
//                _localizedResources = new AppResources();
//                return _localizedResources;
//            }
//        }

//        public static string LocalizedString(string resourceKey)
//        {
//            try
//            {
//                return AppResources.ResourceManager.GetString(resourceKey);
//            }
//            catch (System.Exception)
//            {
//                return "LocalizedString not found";
//            }
//        }
//    }
//}