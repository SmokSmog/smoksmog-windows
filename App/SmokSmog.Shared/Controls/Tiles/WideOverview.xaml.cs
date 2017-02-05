using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls.Tiles
{
    public sealed partial class WideOverview : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowName.
        public static readonly DependencyProperty ShowNameProperty =
            DependencyProperty.Register("ShowName", typeof(bool), typeof(WideOverview), new PropertyMetadata(false));

        public WideOverview()
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