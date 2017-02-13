using System;
using System.Globalization;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP

using Windows.UI;
using Windows.UI.Xaml.Media;

#endif

#if WINDOWS_DESKTOP

using System.Windows.Data;
using System.Windows.Media;

#endif

namespace SmokSmog.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToColor(this string color)
        {
            if (string.IsNullOrWhiteSpace(color) || color[0] != '#')
                throw new ArgumentException(nameof(color));

            color = color.Replace("#", "");
            switch (color.Length)
            {
                case 6:
                    return Color.FromArgb(255,
                        byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber),
                        byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber),
                        byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber));

                case 8:
                    return Color.FromArgb(
                        byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber),
                        byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber),
                        byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber),
                        byte.Parse(color.Substring(6, 2), NumberStyles.HexNumber));
            }
            throw new ArgumentException(nameof(color));
        }

        public static Brush ToBrush(this string color)
        {
            return new SolidColorBrush(color.ToColor());
        }
    }
}