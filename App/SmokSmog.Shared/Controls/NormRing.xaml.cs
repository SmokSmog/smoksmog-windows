using SmokSmog.Globalization;
using SmokSmog.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls
{
    public sealed partial class NormRing : UserControl
    {
        public NormRing()
        {
            this.InitializeComponent();
        }

        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(string), typeof(NormRing), new PropertyMetadata(""));

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(NormRing), new PropertyMetadata(-180d));

        public string Percent
        {
            get { return (string)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percent.
        public static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register("Percent", typeof(string), typeof(NormRing), new PropertyMetadata("0%"));

        public ParameterWithMeasurements ParameterWithMeasurements
        {
            get { return (ParameterWithMeasurements)GetValue(ParameterWithMeasurementsProperty); }
            set { SetValue(ParameterWithMeasurementsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParameterWithMeasurements
        public static readonly DependencyProperty ParameterWithMeasurementsProperty =
            DependencyProperty.Register("ParameterWithMeasurements", typeof(ParameterWithMeasurements), typeof(NormRing), new PropertyMetadata(null, ParameterWithMeasurementsChanged));

        private static void ParameterWithMeasurementsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ring = d as NormRing;
            if (ring != null)
            {
                if (e.NewValue == null)
                {
                    LocalizedStrings LocalizedStrings = new LocalizedStrings();
                    ring.Percent = string.Format(LocalizedStrings.LocalizedString("StringNA"));
                    ring.EndAngle = -180d;
                    return;
                }

                var pwm = ring.ParameterWithMeasurements;
                if (pwm != null)
                {
                    var norm = pwm?.Parameter?.NormValue;
                    var avg = pwm.LastMeasurement?.Average.Value;

                    if (norm.HasValue && avg.HasValue)
                    {
                        double ratio = avg.Value / norm.Value;
                        string format = "{0:0.0}%";

                        if (ratio > 0 && ratio < 1)
                        {
                            ring.EndAngle = ratio * 360d - 180d;
                            ring.Color = "#5ae1d7";
                            if (ratio < 0.1d)
                                format = "{0:0.00}%";
                        }
                        else if (ratio >= 1)
                        {
                            ring.EndAngle = 180d;
                            ring.Color = "#FFf2a21b";
                            if (ratio >= 10)
                                format = "{0:0.}%";
                        }
                        else
                        {
                            ring.EndAngle = -180d;
                            ring.Color = "#5ae1d7";
                        }

                        ring.Percent = string.Format(format, ratio * 100d);
                    }
                }
                else
                {
                    ring.EndAngle = -180d;
                }
            }
        }
    }
}