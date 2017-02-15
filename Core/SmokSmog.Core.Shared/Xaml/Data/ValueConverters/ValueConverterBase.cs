/*
Conditional Compilation Symbols

Windows Universal 10  - WINDOWS_UWP
Windows Store 8.1     - WINDOWS_APP
Windows Phone 8.1     - WINDOWS_PHONE_APP
WPF .net45            - WINDOWS_DESKTOP

*/

using System;
using System.Globalization;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP

using Windows.UI.Xaml.Data;

#endif

#if WINDOWS_DESKTOP

using System.Windows.Data;

#endif

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public abstract class ValueConverterBase : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string cultureOrlanguage)
        {
            if (string.IsNullOrWhiteSpace(cultureOrlanguage))
                cultureOrlanguage = "pl-PL";

            return Convert(value, targetType, parameter, new CultureInfo(cultureOrlanguage));
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage);

        public object ConvertBack(object value, Type targetType, object parameter, string cultureOrlanguage)
        {
            if (string.IsNullOrWhiteSpace(cultureOrlanguage))
                cultureOrlanguage = "pl-PL";

            return ConvertBack(value, targetType, parameter, new CultureInfo(cultureOrlanguage));
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            throw new NotImplementedException();
        }
    }
}