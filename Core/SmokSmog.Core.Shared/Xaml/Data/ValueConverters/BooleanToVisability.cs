using System;
using System.Globalization;
using Windows.UI.Xaml;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class BooleanToVisability : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)

        {
            bool reverse = !string.IsNullOrWhiteSpace(parameter as string) && (parameter.ToString() == "Reverse");
            bool boolValue = (value as Boolean?) ?? false;
            if (reverse) boolValue = !boolValue;
            if (boolValue)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
    }
}