using SmokSmog.Extensions;
using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StringToFirstCharCase : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)

        {
            string stringValue = value as string;
            return !string.IsNullOrWhiteSpace(stringValue) ? stringValue.ToFirstCharCase() : string.Empty;
        }
    }
}