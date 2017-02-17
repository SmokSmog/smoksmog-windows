using GalaSoft.MvvmLight;
using SmokSmog.Services.Storage;
using System;
using System.Text;
using Windows.ApplicationModel;

namespace SmokSmog.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;

        public MainViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            if (_settingsService.LastLaunchedVersion == null)
                IsFirstRun = true;

            if (_settingsService.LastLaunchedVersion != ApplicationVersion)
                IsFirstRunAfterUpdate = true;

            _settingsService.LastLaunchedVersion = ApplicationVersion;
        }

        public Version ApplicationVersion
        {
            get
            {
                if (IsInDesignModeStatic)
                    return new Version(2, 2, 224);

                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;
                return new Version(version.Major, version.Minor, version.Build);
            }
        }

        public string ApplicationVersionString
            => string.Format("{0}.{1}.{2}", ApplicationVersion.Major, ApplicationVersion.Minor, ApplicationVersion.Build);

        public string ApplicationName
        {
            get
            {
                if (IsInDesignModeStatic)
                    return "SmokSmog";

                var package = Package.Current;
                var invariantPackageName = package.Id.Name.ToLowerInvariant();

                if (invariantPackageName.Contains("alpha"))
                    return "SmokSmog Aplha";

                if (invariantPackageName.Contains("beta"))
                    return "SmokSmog Beta";

                return "SmokSmog";
            }
        }

        public bool IsFirstRun { get; private set; }

        public bool IsFirstRunAfterUpdate { get; private set; }

        public string Changelog
        {
            get
            {
                //TODO - figure out how to provide proper change log handling, this is only for time saving for release 2.0.2xx Beta
                var changeLogSb = new StringBuilder();

                changeLogSb.AppendLine($"Wersja {ApplicationVersionString}");
                changeLogSb.AppendLine();

                changeLogSb.AppendLine("NOWOŚCI:");
                changeLogSb.AppendLine("-Live Tile dla Stacji domowej");
                changeLogSb.AppendLine("-Dodano obsługę PM₂ ̦ ₅");
                changeLogSb.AppendLine("-Dodano stronę ustawień");
                changeLogSb.AppendLine();

                changeLogSb.AppendLine("Poprawki:");
                changeLogSb.AppendLine("-Naprwione menu kontekstowe");
                changeLogSb.AppendLine("-Drobne poprawki UI");
                changeLogSb.AppendLine("-Poprawki w szukajce :D");

                return changeLogSb.ToString();
            }
        }
    }
}