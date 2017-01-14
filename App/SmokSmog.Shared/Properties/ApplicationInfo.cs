using GalaSoft.MvvmLight;
using Windows.ApplicationModel;

namespace SmokSmog.Properties
{
    public class ApplicationInfo
    {
        public string Name
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return "SmokSmog";
                }

                Package package = Package.Current;
                var invariantPackageName = package.Id.Name.ToLowerInvariant();

                if (invariantPackageName.Contains("alpha"))
                    return "SmokSmog Aplha";

                if (invariantPackageName.Contains("beta"))
                    return "SmokSmog Beta";

                return "SmokSmog";
            }
        }

        public string Version
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return "2.0.0";
                }

                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;

                return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            }
        }
    }
}