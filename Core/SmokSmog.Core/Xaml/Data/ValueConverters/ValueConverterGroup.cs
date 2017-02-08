/*
Conditional Compilation Symbols

Windows Universal 10  - WINDOWS_UWP
Windows Store 8.1     - WINDOWS_APP
Windows Phone 8.1     - WINDOWS_PHONE
WPF .net45            - WINDOWS_DESKTOP

*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE

using Windows.UI.Xaml.Data;

#endif

#if WINDOWS_DESKTOP

using System.Windows.Data;

#endif

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class ValueConverterGroup : List<ValueConverterBase>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string cultureOrlanguage)
        {
            if (string.IsNullOrWhiteSpace(cultureOrlanguage))
                cultureOrlanguage = "pl-PL";

            return Convert(value, targetType, parameter, new CultureInfo(cultureOrlanguage));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, cultureOrlanguage));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string cultureOrlanguage)
        {
            if (string.IsNullOrWhiteSpace(cultureOrlanguage))
                cultureOrlanguage = "pl-PL";

            return ConvertBack(value, targetType, parameter, new CultureInfo(cultureOrlanguage));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            throw new NotImplementedException();
        }
    }
}