using SmokSmog.Model;
using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class AqiToImage : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            var square = parameter?.Equals("square") ?? false;

            AirQualityIndex aqi = value as AirQualityIndex;

            if (aqi != null)
            {
                if (square)
                    return new Uri($"ms-appx:///Assets/Notification/{aqi.Info.Level}-square.png");
                return new Uri($"ms-appx:///Assets/Notification/{aqi.Info.Level}.png");
            }
            return new Uri("");
        }
    }
}