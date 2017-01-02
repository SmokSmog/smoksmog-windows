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

using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

#endif

#if WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endif

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class AqiIntToBrush : IValueConverter
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

        public object Convert(object value, Type targetType, object parameter, string cultureOrlanguage)
#elif WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
#endif
        {
            int? val = value as int?;

            byte alpha = 127;

            if (val.HasValue)
            {
                Color color = Color.FromArgb(alpha, 200, 200, 200);
                switch (val)
                {
                    case 1: color = Color.FromArgb(alpha, 74, 100, 51); break;
                    case 2: color = Color.FromArgb(alpha, 176, 221, 16); break;
                    case 3: color = Color.FromArgb(alpha, 255, 217, 17); break;
                    case 4: color = Color.FromArgb(alpha, 229, 129, 0); break;
                    case 5: color = Color.FromArgb(alpha, 229, 0, 0); break;
                    case 6: color = Color.FromArgb(alpha, 153, 0, 0); break;
                    default: break;
                }

                return new SolidColorBrush(color);
            }
            return null;
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