﻿#if WINDOWS_UWP

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
                    return UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Mouse
                        ? DeviceType.Desktop
                        : DeviceType.Tablet;

                case "Windows.Universal":
                    return DeviceType.IoT;

                case "Windows.Team":
                    return DeviceType.SurfaceHub;

                default:
                    //TODO: extend for Xbox, Hololeans
                    return DeviceType.Other;
            }

#endif
        }

        
    }
}