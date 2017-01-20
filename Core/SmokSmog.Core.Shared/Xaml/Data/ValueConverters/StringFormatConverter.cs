using SmokSmog.Globalization;
using System;
using Windows.UI.Xaml.Data;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StringDigitFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}