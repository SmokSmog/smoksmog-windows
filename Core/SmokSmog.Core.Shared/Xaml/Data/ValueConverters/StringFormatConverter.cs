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

            double val = 0;
            bool isDouble = double.TryParse(value.ToString(), out val);

            if (isDouble)
            {
                if (double.IsPositiveInfinity(val))
                    return "∞";

                if (double.IsNegativeInfinity(val))
                    return "-∞";

                return string.Format(((parameter as string) ?? "{0}").Replace('[', '{').Replace(']', '}'), val);
            }

            return LocalizedStrings.LocalizedString("StringNA");
        }
    }
}