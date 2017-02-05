using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StringFormatConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            return value == null ? string.Empty : string.Format(((parameter as string) ?? "{0}").Replace('[', '{').Replace(']', '}'), value);
        }
    }
}