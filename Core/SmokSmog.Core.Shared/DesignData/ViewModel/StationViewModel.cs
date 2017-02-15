using SmokSmog.Model;
using SmokSmog.ViewModel;
using System.Collections.Generic;

namespace SmokSmog.DesignData.ViewModel
{
    public class StationViewModel
    {
        public StationViewModel()
        {
            Station = Station.Sample;
            var api = new Services.ApiDataProvider();
            var @params = api.GetParameters(Station);

            foreach (var parameter in @params)
            {
                AqiComponents.Add(new ParameterViewModel(Station, parameter));
                Parameters.Add(new ParameterViewModel(Station, parameter));
            }

            AirQualityIndex = AirQualityIndex.Unavaible;
        }

        public AirQualityIndex AirQualityIndex { get; }

        public List<ParameterViewModel> AqiComponents { get; } = new List<ParameterViewModel>();

        public bool IsValidStation => true;

        public List<ParameterViewModel> Parameters { get; } = new List<ParameterViewModel>();

        public Station Station { get; }
    }
}