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
            DataContextChanged += OnDataContextChanged;
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
            DependencyProperty.Register("Percent", typeof(string), typeof(NormRing), new PropertyMetadata(LocalizedStrings.LocalizedString("StringNA")));

        public ParameterViewModel Parameter
        {
            get { return (ParameterViewModel)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParameterWithMeasurements
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(nameof(Parameter), typeof(ParameterViewModel), typeof(NormRing), new PropertyMetadata(null, ParameterChanged));

        private static void ParameterChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var ring = sender as NormRing;
            if (ring != null)
            {
                ring.DataContext = args.NewValue;
            }
        }

        private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var pwm = args.NewValue as ParameterViewModel;
            var norm = pwm?.Parameter?.Norm;
            double? average = null;
            if (norm != null)
            {
                average = pwm?.Latest[norm.Aggregation];
            }

            if (norm == null || !average.HasValue)
            {
                Percent = string.Format(LocalizedStrings.LocalizedString("StringNA"));
                EndAngle = -180d;
                return;
            }

            double ratio = average.Value / norm.Value;
            string format = "{0:0.0}%";

            if (ratio > 0 && ratio < 1)
            {
                EndAngle = ratio * 360d - 180d;
                Color = "#FF5AE1D7";
                if (ratio < 0.1d)
                    format = "{0:0.00}%";
            }
            else if (ratio >= 1)
            {
                EndAngle = 180d;
                Color = "#FFf2A21B";
                if (ratio >= 10)
                    format = "{0:0.}%";
            }
            else
            {
                EndAngle = -180d;
                Color = "#FF5AE1D7";
            }

            Percent = string.Format(format, ratio * 100d);
        }
    }
}