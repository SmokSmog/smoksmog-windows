using System;

namespace SmokSmog.Model
{
    public enum AirQualityLevel : int
    {
        NotAvailable = -1,
        VeryGood = 0,
        Good = 1,
        Moderate = 2,
        Sufficient = 3,
        Bad = 4,
        VeryBad = 5,
    }

    public class AirQualityIndex
    {
        private AirQualityIndex()
        {
            Info = AirQualityInfo.Factory(AirQualityLevel.NotAvailable);
        }

        private AirQualityIndex(double? value)
        {
            Value = value;

            if (!value.HasValue) Info = AirQualityInfo.Factory(AirQualityLevel.NotAvailable);

            foreach (AirQualityLevel item in Enum.GetValues(typeof(AirQualityLevel)))
            {
                var info = AirQualityInfo.Factory(item);
                if (info.Minimum < Value.Value && Value <= info.Maximum)
                {
                    Info = info;
                    break;
                }
            }
        }

        public AirQualityInfo Info { get; }

        public static AirQualityIndex Unavaible => new AirQualityIndex();

        public double? Value { get; } = null;

        public static AirQualityIndex CalculateAirQualityIndex(ParameterType parameterType, double? parameterValue)
        {
            if (!parameterValue.HasValue)
                return new AirQualityIndex();

            double index = parameterValue.Value * 5;
            switch (parameterType)
            {
                // Polish Air Quality Index based on WIOŚ algorithm helpful links :
                // http://aqicn.org/faq/2015-09-03/air-quality-scale-in-poland/pl/ http://monitoring.krakow.pios.gov.pl/

                case ParameterType.SO2: index /= 350d; break; // Confirmed
                case ParameterType.NO2: index /= 200d; break; // Confirmed
                case ParameterType.CO: index /= 10000d; break; // Most Probably
                case ParameterType.O3: index /= 120d; break; // Confirmed
                case ParameterType.PM10: index /= 100d; break; // Confirmed
                case ParameterType.C6H6: index /= 40d; break; // Confirmed
                case ParameterType.PM25: index /= 60d; break; // Confirmed

                case ParameterType.NOx:
                case ParameterType.NO:
                case ParameterType.UNKNOWN:
                default:
                    return new AirQualityIndex();
            }

            return new AirQualityIndex(index);
        }
    }
}