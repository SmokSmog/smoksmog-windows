/*
Conditional Compilation Symbols

Windows Universal 10  - WINDOWS_UWP
Windows Store 8.1     - WINDOWS_APP
Windows Phone 8.1     - WINDOWS_PHONE_APP
Windows Phone 8.0     - WINDOWS_PHONE
WPF .net45            - WINDOWS_DESKTOP

*/

#if !PORTABLE

using System;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

using Windows.UI.Xaml.Data;

#endif

#if WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endif

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StationGroupingModeEnumToString : IValueConverter
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

        public object Convert(object value, Type targetType, object parameter, string cultureOrlanguage)
#elif WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
#endif
        {
            if (value is ViewModel.StationGroupingModeEnum)
            {
                switch ((ViewModel.StationGroupingModeEnum)value)
                {
                    case ViewModel.StationGroupingModeEnum.Name:
                        return Globalization.LocalizedStrings.LocalizedString("StationSortModeEnumName");

                    //case ViewModel.StationGroupingModeEnum.City:
                    //    return Globalization.LocalizedStrings("StationSortModeEnumCity");

                    case ViewModel.StationGroupingModeEnum.Province:
                        return Globalization.LocalizedStrings.LocalizedString("StationSortModeEnumProvince");
                }
            }
            return "String not found";
        }

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

        public object ConvertBack(object value, Type targetType, object parameter, string cultureOrlanguage)
#elif WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
#endif
        {
            throw new NotSupportedException();
        }
    }
}

#endif