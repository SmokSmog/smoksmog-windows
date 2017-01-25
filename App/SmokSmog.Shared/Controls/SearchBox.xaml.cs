using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls
{
    public sealed partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            this.InitializeComponent();
        }

        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register("SearchString", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty, TextPropertyChangedCallback));

        private static void TextPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var sb = sender as SearchBox;
            if (sb != null)
                sb.SearchTextBox.Text = args.NewValue.ToString();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchString = SearchTextBox.Text;

#if WINDOWS_PHONE
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                CancelationButton.Visibility = Visibility.Visible;
            }
            else
            {
                CancelationButton.Visibility = Visibility.Collapsed;
            }
#endif
        }

        private void CancelationButton_Click(object sender, RoutedEventArgs e)
        {
            SearchString = string.Empty;
        }
    }
}