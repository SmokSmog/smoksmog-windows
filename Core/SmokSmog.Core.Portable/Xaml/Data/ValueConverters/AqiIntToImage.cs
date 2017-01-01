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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

#endif

#if WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endif

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class AqiIntToImage : IValueConverter
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

        public object Convert(object value, Type targetType, object parameter, string cultureOrlanguage)
#elif WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
#endif
        {
            int? val = value as int?;
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/0.png"));
            if (val.HasValue)
            {
                switch (val)
                {
                    case 1: img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/1.png")); break;
                    case 2: img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/2.png")); break;
                    case 3: img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/3.png")); break;
                    case 4: img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/4.png")); break;
                    case 5: img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/5.png")); break;
                    case 6: img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/6.png")); break;
                    //case 6: logo = new BitmapImage(new Uri("pack://application:,,,/SmokSmog.Core.Common;component/Assets/AQI/6.png")); break;
                    default: img = new BitmapImage(new Uri("ms-appx:///SmokSmog.Core.Common/Assets/AQI/0.png")); break;
                }
            }
            return img;
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