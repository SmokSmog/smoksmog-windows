using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SmokSmog.Controls
{
    public sealed partial class SvgImage : UserControl
    {
        public SvgImage()
        {
            InitializeComponent();
        }

        public Path Path
        {
            get { return (Path)GetValue(PathProperty); }
            set
            {
                SetValue(PathProperty, value);
                var a = vb.Child;
                if (value != null)
                    vb.Child = new Path() { Data = value.Data, Width = value.Width, Height = value.Height };
                else
                    vb.Child = null;
            }
        }

        private Geometry Geom => Path.Data;

        // Using a DependencyProperty as the backing store for Path.
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(Path), typeof(SvgImage), new PropertyMetadata(new Path()));
    }
}