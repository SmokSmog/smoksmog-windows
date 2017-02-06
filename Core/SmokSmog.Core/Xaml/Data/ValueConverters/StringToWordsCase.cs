using System;
using System.Globalization;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StringToWordsCase : ValueConverterBase

    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
            => Convert(value, targetType, parameter, cultureOrlanguage.EnglishName ?? string.Empty);
    }
}