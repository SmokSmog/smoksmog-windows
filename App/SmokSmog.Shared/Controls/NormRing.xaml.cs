using SmokSmog.Globalization;
using SmokSmog.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls
{
    public sealed partial class NormRing : UserControl
    {
        public NormRing()
        {
            this.InitializeComponent();

            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
                Parameter = DataContext as Parameter;
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

        public Parameter Parameter
        {
            get { return (Parameter)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParameterWithMeasurements
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(nameof(Parameter), typeof(Parameter), typeof(NormRing), new PropertyMetadata(null, ParameterChanged));

        private static void ParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ring = d as NormRing;
            if (ring != null)
            {
                if (e.NewValue == null)
                {
                    ring.Percent = string.Format(LocalizedStrings.LocalizedString("StringNA"));
                    ring.EndAngle = -180d;
                    return;
                }

                var pwm = ring.Parameter;
                if (pwm != null)
                {
                    var norm = pwm?.Norm;
                    var avg = pwm.Current?.Value;

                    if (norm != null && norm.Aggregation == AggregationType.Avg1Hour && avg.HasValue)
                    {
                        double ratio = avg.Value / norm.Value;
                        string format = "{0:0.0}%";

                        if (ratio > 0 && ratio < 1)
                        {
                            ring.EndAngle = ratio * 360d - 180d;
                            ring.Color = "#FF5AE1D7";
                            if (ratio < 0.1d)
                                format = "{0:0.00}%";
                        }
                        else if (ratio >= 1)
                        {
                            ring.EndAngle = 180d;
                            ring.Color = "#FFf2A21B";
                            if (ratio >= 10)
                                format = "{0:0.}%";
                        }
                        else
                        {
                            ring.EndAngle = -180d;
                            ring.Color = "#FF5AE1D7";
                        }

                        ring.Percent = string.Format(format, ratio * 100d);
                    }
                    else
                    {
                        ring.Percent = string.Format(LocalizedStrings.LocalizedString("StringNA"));
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