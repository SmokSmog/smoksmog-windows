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
        private readonly AirQualityInfo _info;

        private AirQualityIndex()
        {
            _info = AirQualityInfo.Factory(AirQualityLevel.NotAvailable);
            Date = DateTime.Now;
            DateUtc = DateTime.UtcNow;
        }

        private AirQualityIndex(Measurement measurement, double? value = null)
        {
            Value = value;

            if (!value.HasValue) _info = AirQualityInfo.Factory(AirQualityLevel.NotAvailable);

            foreach (AirQualityLevel item in Enum.GetValues(typeof(AirQualityLevel)))
            {
                var info = AirQualityInfo.Factory(item);
                if (!(info.Minimum < Value) || !(Value <= info.Maximum)) continue;
                _info = info;
                break;
            }

            Measurement = measurement;
            Date = measurement.Date;
            DateUtc = measurement.DateUtc;

            Parameter = measurement.Parameter;
        }

        public string Color => _info.Color;
        public DateTime Date { get; }
        public DateTime DateUtc { get; }
        public AirQualityLevel Level => _info.Level;
        public Measurement Measurement { get; }
        public Parameter Parameter { get; }
        public string Text => _info.Text;
        public static AirQualityIndex Unavaible => new AirQualityIndex();
        public double? Value { get; } = null;

        public static AirQualityIndex CalculateAirQualityIndex(Measurement measurement)
        {
            if (measurement == null)
                return new AirQualityIndex();

            ParameterType parameterType = measurement.Parameter.Type;
            double? parameterValue = measurement.Value;

            if (measurement.Aggregation != AggregationType.Avg1Hour)
                return new AirQualityIndex();

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

            return new AirQualityIndex(measurement, index);
        }
    }
}