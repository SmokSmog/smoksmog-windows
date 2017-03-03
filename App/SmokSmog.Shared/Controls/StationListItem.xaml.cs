using SmokSmog.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmokSmog.Controls
{
    public sealed partial class StationListItem : UserControl
    {
        // Using a DependencyProperty as the backing store for Distance.
        public static readonly DependencyProperty DistanceProperty =
            DependencyProperty.Register("Distance", typeof(string), typeof(StationListItem), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for SearchString.
        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register("SearchString", typeof(string), typeof(StationListItem), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for Station.
        public static readonly DependencyProperty StationProperty =
            DependencyProperty.Register("Station", typeof(Station), typeof(StationListItem), new PropertyMetadata(new Station(-1)));

        public StationListItem()
        {
            this.InitializeComponent();
        }

        public string Distance
        {
            get { return (string)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }

        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        public Station Station
        {
            get { return (Station)GetValue(StationProperty); }
            set { SetValue(StationProperty, value); }
        }
    }
}