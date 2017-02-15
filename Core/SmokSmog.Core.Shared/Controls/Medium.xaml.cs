using SmokSmog.Xaml.Data.ValueConverters;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls.Tiles
{
    public sealed partial class Medium : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowName.
        public static readonly DependencyProperty ShowNameProperty =
            DependencyProperty.Register("ShowName", typeof(bool), typeof(Medium), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for Site.
        public static readonly DependencyProperty SiteProperty =
            DependencyProperty.Register("Site", typeof(TileSite), typeof(Medium), new PropertyMetadata(TileSite.Front, TileSiteChangedCallback));

        public Medium()
        {
            this.InitializeComponent();
        }

        public bool ShowName
        {
            get { return (bool)GetValue(ShowNameProperty); }
            set { SetValue(ShowNameProperty, value); }
        }

        public TileSite Site
        {
            get { return (TileSite)GetValue(SiteProperty); }
            set { SetValue(SiteProperty, value); }
        }

        private static void TileSiteChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var wide = sender as Medium;
            if (wide == null) return;

            var tileSite = args.NewValue as TileSite?;
            if (!tileSite.HasValue) return;

            if (tileSite == TileSite.Front)
            {
                wide.FrontContent.Visibility = Visibility.Visible;
                wide.BackContent.Visibility = Visibility.Collapsed;
            }
            else
            {
                wide.FrontContent.Visibility = Visibility.Collapsed;
                wide.BackContent.Visibility = Visibility.Visible;
            }
        }

        public AqiToImage AqiToImage { get; } = new AqiToImage();
        public BooleanToVisability BooleanToVisability { get; } = new BooleanToVisability();
        public StringFormatConverter StringFormatConverter { get; } = new StringFormatConverter();
        public NumericFormatConverter NumericFormatConverter { get; } = new NumericFormatConverter();
    }
}