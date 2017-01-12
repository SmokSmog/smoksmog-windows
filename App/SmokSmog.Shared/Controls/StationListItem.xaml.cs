using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SmokSmog.Controls
{
    public sealed partial class StationListItem : UserControl
    {
        public StationListItem()
        {
            this.InitializeComponent();
        }

        //public Model.Station Station
        //{
        //    get { return (Model.Station)GetValue(StationProperty); }
        //    set { SetValue(StationProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Station.
        //public static readonly DependencyProperty StationProperty =
        //    DependencyProperty.Register("Station", typeof(Model.Station), typeof(StationListItem), new PropertyMetadata(Model.Station.Empty, OnStationPropertyChanged));

        //private static void OnStationPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        //{
        //    StationListItem stationListItem = sender as StationListItem;
        //    if (stationListItem != null)
        //    {
        //    }
        //}

        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchString.
        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register("SearchString", typeof(string), typeof(StationListItem), new PropertyMetadata(string.Empty));
    }
}