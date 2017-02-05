using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls.Tiles
{
    public sealed partial class WideDetails : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowName.
        public static readonly DependencyProperty ShowNameProperty =
            DependencyProperty.Register("ShowName", typeof(bool), typeof(WideDetails), new PropertyMetadata(false));

        public WideDetails()
        {
            this.InitializeComponent();
        }

        public bool ShowName
        {
            get { return (bool)GetValue(ShowNameProperty); }
            set { SetValue(ShowNameProperty, value); }
        }
    }
}