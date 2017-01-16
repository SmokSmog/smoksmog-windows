using SmokSmog.Model;
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
            DependencyProperty.Register("EndAngle", typeof(double), typeof(AqiRing), new PropertyMetadata(-50));

        public AirQualityIndex AQI

        {
            get { return (AirQualityIndex)GetValue(AQIProperty); }
            set { SetValue(AQIProperty, value); }
        }

        public static readonly DependencyProperty AQIProperty =
            DependencyProperty.Register("AQI", typeof(AirQualityIndex), typeof(AqiRing), new PropertyMetadata(AirQualityIndex.Unavaible));
    }
}