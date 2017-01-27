#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE

using Windows.UI.Xaml;

#endif

#if WINDOWS_DESKTOP

using System.Windows;

#endif

namespace SmokSmog.Xaml.Data
{
    public class ArrayItem : DependencyObject
    {
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(ArrayItem), new PropertyMetadata(new object()));
    }
}