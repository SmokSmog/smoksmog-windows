using SmokSmog.Extensions;
using SmokSmog.Model;
using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class AqiToBrush : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            var aqi = value as AirQualityIndex;
            return aqi?.Color.ToBrush();
        }
    }
}