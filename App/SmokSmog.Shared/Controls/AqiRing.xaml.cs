﻿using SmokSmog.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls
{
    public sealed partial class AqiRing : UserControl
    {
        public AqiRing()
        {
            this.InitializeComponent();
        }

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(AqiRing), new PropertyMetadata(-150d));

        public AirQualityIndex AQI

        {
            get { return (AirQualityIndex)GetValue(AQIProperty); }
            set { SetValue(AQIProperty, value ?? AirQualityIndex.Unavaible); }
        }

        public static readonly DependencyProperty AQIProperty =
            DependencyProperty.Register("AQI", typeof(AirQualityIndex), typeof(AqiRing), new PropertyMetadata(AirQualityIndex.Unavaible, AQIChanged));

        private static void AQIChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ring = d as AqiRing;
            if (ring != null)
            {
                if (e.NewValue == null)
                {
                    ring.AQI = AirQualityIndex.Unavaible;
                    ring.EndAngle = -150d;
                    return;
                }

                var aqi = ring.AQI;

                if (aqi.Value.HasValue)
                {
                    if (aqi.Value > 0 && aqi.Value < 10)
                    {
                        ring.EndAngle = aqi.Value.Value / 10d * 300d - 150d;
                    }
                    else if (aqi.Value >= 10)
                    {
                        ring.EndAngle = 150d;
                    }
                    else
                        ring.EndAngle = -150;
                }
                else
                {
                    ring.EndAngle = -150d;
                }
            }
        }
    }
}