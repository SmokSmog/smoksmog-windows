using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using Windows.ApplicationModel.Resources;

namespace SmokSmog.Globalization
{
    public class WinRTResourceManager : ResourceManager
    {
        private readonly ResourceLoader _resourceLoader;

        private WinRTResourceManager(string baseName, Assembly assembly) : base(baseName, assembly)
        {
            _resourceLoader = ResourceLoader.GetForViewIndependentUse(baseName);
        }

        /// <summary>
        /// </summary>
        /// <param name="appResourcesType">RESX Generated AppResources Class Type</param>
        public static void InjectIntoResxGeneratedAppResources(Type appResourcesType)
        {
            string name = appResourcesType.FullName;
            Assembly assembly = appResourcesType.GetTypeInfo().Assembly;
            ResourceManager resourceManager = new WinRTResourceManager(name, assembly);

            var resourceMan = appResourcesType
                .GetRuntimeFields()
                .First(m => m.Name == "resourceMan");

            resourceMan.SetValue(null, resourceManager);
        }

        public override string GetString(string name, CultureInfo culture)
        {
            return _resourceLoader.GetString(name);
        }
    }
}