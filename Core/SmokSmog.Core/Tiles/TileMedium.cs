using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SmokSmog.Tiles
{
    [TemplatePart(Name = IconPartName, Type = typeof(Image))]
    public class TileMedium : Control
    {
        public const string IconPartName = "PART_Icon";

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(TileMedium),
                new PropertyMetadata(new BitmapImage(new Uri("ms-appx:///SmokSmog.Core/Assets/Notification/Bad-square.png"))));

        public TileMedium() : base()
        {
            this.DefaultStyleKey = typeof(TileMedium);
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            var image = GetTemplateChild(IconPartName) as Image;
            if (image != null && Icon != null) image.Source = Icon;
        }
    }
}