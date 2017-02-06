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
            bool boolValue = (value as bool?) ?? false;
            if (reverse) boolValue = !boolValue;
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}