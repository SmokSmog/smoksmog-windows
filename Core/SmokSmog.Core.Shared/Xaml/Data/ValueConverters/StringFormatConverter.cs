using System;
using Windows.UI.Xaml.Data;

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.Format(((parameter as string) ?? "{0}").Replace('[', '{').Replace(']', '}'), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}