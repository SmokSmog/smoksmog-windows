using SmokSmog.Model;
using System;
using System.Globalization;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE

using Windows.UI.Xaml.Media.Imaging;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class AqiToImage : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            var square = parameter?.Equals("square") ?? false;
            var aqi = value as AirQualityIndex;

            if (aqi != null)
            {
                var uri = square
                    ? new Uri($"ms-appx:///Assets/Notification/{aqi.Info.Level}-square.png")
                    : new Uri($"ms-appx:///Assets/Notification/{aqi.Info.Level}.png");
                return new BitmapImage(uri);
            }

            return new BitmapImage();
        }
    }
}

#endif

#if WINDOWS_DESKTOP

whan include to WPF add reference anf implementation

#endif