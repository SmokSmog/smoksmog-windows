using System;
using System.Globalization;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE

using Windows.UI;
using Windows.UI.Xaml.Media;

#endif

#if WINDOWS_DESKTOP

using System.Windows.Data;
using System.Windows.Media;

#endif

namespace SmokSmog.Xaml.Data.ValueConverters
{
    public class AqiIntToBrush : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo cultureOrlanguage)
        {
            int? val = value as int?;

            byte alpha = 127;

            if (val.HasValue)
            {
                Color color = Color.FromArgb(alpha, 200, 200, 200);
                switch (val)
                {
                    case 1: color = Color.FromArgb(alpha, 74, 100, 51); break;
                    case 2: color = Color.FromArgb(alpha, 176, 221, 16); break;
                    case 3: color = Color.FromArgb(alpha, 255, 217, 17); break;
                    case 4: color = Color.FromArgb(alpha, 229, 129, 0); break;
                    case 5: color = Color.FromArgb(alpha, 229, 0, 0); break;
                    case 6: color = Color.FromArgb(alpha, 153, 0, 0); break;
                    default: break;
                }

                return new SolidColorBrush(color);
            }
            return null;
        }
    }
}