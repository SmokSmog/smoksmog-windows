using GalaSoft.MvvmLight;
using SmokSmog.Model;
using System;
using System.Collections.Generic;

namespace SmokSmog.ViewModel
{
    public class AqiInformationViewModel : ViewModelBase
    {
        public AqiInformationViewModel()
        {
            List<AirQualityInfo> airQualityInfos = new List<AirQualityInfo>();

            foreach (AirQualityLevel item in Enum.GetValues(typeof(AirQualityLevel)))
            {
                if (item == AirQualityLevel.NotAvailable)
                    continue;

                var info = AirQualityInfo.Factory(item);
                airQualityInfos.Add(info);
            }

            AirQualityInfos = airQualityInfos;
        }

        public List<AirQualityInfo> AirQualityInfos { get; } = new List<AirQualityInfo>();
    }
}