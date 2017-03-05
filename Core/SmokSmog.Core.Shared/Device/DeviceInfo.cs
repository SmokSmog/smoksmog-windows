using System;
using Windows.Graphics.Display;

#if WINDOWS_UWP

using Windows.System.Profile;
using Windows.UI.ViewManagement;

#endif

namespace SmokSmog.Device
{
    public enum DeviceType
    {
        Phone,
        Desktop,
        Xbox,
        Tablet,
        Hololeans,
        IoT,
        SurfaceHub,
        Other
    }

    public static class DeviceInfo
    {
        public static float GetDeviceDpi()
        {
            return 96f * GetScreenScaling();
        }

        /// <summary>
        ///
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/23267992/detecting-current-device-in-windows-universal-app"/>
        /// <seealso cref="https://gist.github.com/wagonli/40d8a31bd0d6f0dd7a5d"/>
        /// <returns></returns>
        public static DeviceType GetDeviceType()
        {
#if WINDOWS_PHONE_APP

            return DeviceType.Phone;

#elif WINDOWS_APP

            return DeviceType.Desktop;

#elif WINDOWS_UWP

            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                case "Windows.Mobile":
                    return DeviceType.Phone;

                case "Windows.Desktop":
                    // For background Task CurrentView doesn't exist
                    //return UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Mouse
                    //    ? DeviceType.Desktop
                    //    : DeviceType.Tablet;
                    return DeviceType.Desktop;

                case "Windows.Universal":
                    return DeviceType.IoT;

                case "Windows.Team":
                    return DeviceType.SurfaceHub;

                default:
                    //TODO: extend for Xbox, Hololens
                    return DeviceType.Other;
            }

#endif
        }

        /// <summary>
        /// 1.0  = 100%
        /// 1.25 = 125%
        /// </summary>
        /// <returns></returns>
        public static float GetScreenScaling()
        {
            float scale = 1.0f; // 100%
            try
            {
#if WINDOWS_APP
                scale = (int)DisplayInformation.GetForCurrentView().ResolutionScale / 100f;
#elif WINDOWS_UWP || WINDOWS_PHONE_APP
                scale = (float)DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
#endif
                Services.ServiceLocator.Current.SettingsService.LastScreenScaling = scale;
            }
            catch (Exception)
            {
                scale = Services.ServiceLocator.Current.SettingsService.LastScreenScaling;
            }

            return scale;
        }
    }
}