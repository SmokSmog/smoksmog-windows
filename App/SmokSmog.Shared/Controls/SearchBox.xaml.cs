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

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty));

        public event TextChangedEventHandler TextChanged
        {
            add { SearchTextBox.TextChanged += value; }
            remove { SearchTextBox.TextChanged -= value; }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
#if WINDOWS_PHONE
            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                CancelationButton.Visibility = Visibility.Visible;
            }
            else
            {
                CancelationButton.Visibility = Visibility.Collapsed;
            }
#endif
            Text = SearchTextBox.Text ?? string.Empty;
        }

        private void CancelationButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
        }
    }
}