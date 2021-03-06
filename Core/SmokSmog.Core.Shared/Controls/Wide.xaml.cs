﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SmokSmog.Tiles;

namespace SmokSmog.Controls.Tiles
{
    public sealed partial class Wide : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowName.
        public static readonly DependencyProperty ShowNameProperty =
            DependencyProperty.Register("ShowName", typeof(bool), typeof(Wide), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for Site.
        public static readonly DependencyProperty SiteProperty =
            DependencyProperty.Register("Site", typeof(TileSite), typeof(Wide), new PropertyMetadata(TileSite.Front, TileSiteChangedCallback));

        public Wide()
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
            var wide = sender as Wide;
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
    }
}