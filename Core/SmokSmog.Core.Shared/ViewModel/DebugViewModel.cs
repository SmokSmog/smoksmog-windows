using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;

namespace SmokSmog.ViewModel
{
    public class DebugViewModel : ViewModelBase
    {
        public DebugViewModel()
        {
            var model = new PlotModel { Title = "", Background = OxyColors.Transparent, IsLegendVisible = false, };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0d });

            var lineSeries = new LineSeries { Title = "LineSeries", MarkerType = MarkerType.Circle };

            var random = new Random();
            for (int i = 0; i < 24; i++)
            {
                lineSeries.Points.Add(DateTimeAxis.CreateDataPoint(DateTime.Now.AddHours(i), random.NextDouble()));
            }

            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddHours(24)),
                StringFormat = "HH:mm"
            });

            model.Series.Add(lineSeries);
            this.Model = model;
        }

        public PlotModel Model { get; private set; }

        private async void Load()
        {
            var vml = new ViewModelLocator();
            var stationViewModel = vml.StationViewModel;

            await stationViewModel.SetStationAsync(4);

            //stationViewModel.Parameters[0].LoadData()
        }
    }
}