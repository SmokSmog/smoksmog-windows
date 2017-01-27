using SmokSmog.Globalization;
using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StringDigitFormatConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            if (value == null)
            {
                return LocalizedStrings.LocalizedString("StringNA");
            }

            if (value is double && double.IsPositiveInfinity((double)value))
                return "∞";

            if (value is double && double.IsNegativeInfinity((double)value))
                return "-∞";

            return string.Format(((parameter as string) ?? "{0}").Replace('[', '{').Replace(']', '}'), value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            return null;
        }
    }
}