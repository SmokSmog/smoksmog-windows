using SmokSmog.Model;
using SmokSmog.ViewModel;
using System.Collections.Generic;

namespace SmokSmog.DesignData.ViewModel
{
    public class StationViewModel : SmokSmog.ViewModel.StationViewModel
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

        public new AirQualityIndex AirQualityIndex { get; }

        public new List<ParameterViewModel> AqiComponents { get; } = new List<ParameterViewModel>();

        public new bool IsValidStation => true;

        public new List<ParameterViewModel> Parameters { get; } = new List<ParameterViewModel>();

        public new Station Station { get; }
    }
}