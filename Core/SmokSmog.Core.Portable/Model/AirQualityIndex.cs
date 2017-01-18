namespace SmokSmog.Model
{
    public enum AirQualityLevel : int
    {
        Unavaible = -1,
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
        }

        private AirQualityIndex(double? value)
        {
            Value = value;

            if (!value.HasValue) Level = AirQualityLevel.Unavaible;
            else if (value <= 1) Level = AirQualityLevel.VeryGood;
            else if (value <= 3) Level = AirQualityLevel.Good;
            else if (value <= 5) Level = AirQualityLevel.Moderate;
            else if (value <= 7) Level = AirQualityLevel.Sufficient;
            else if (value <= 10) Level = AirQualityLevel.Bad;
            else if (value > 10) Level = AirQualityLevel.VeryBad;
        }

        public static AirQualityIndex Unavaible => new AirQualityIndex();

        public string Color
        {
            get
            {
                switch (Level)
                {
                    case AirQualityLevel.VeryGood: return "#FF00b050";
                    case AirQualityLevel.Good: return "#FF92D050";
                    case AirQualityLevel.Moderate: return "#FFFFFF00";
                    case AirQualityLevel.Sufficient: return "#FFFFC000";
                    case AirQualityLevel.Bad: return "#FFFF0000";
                    case AirQualityLevel.VeryBad: return "#FFC00000";
                    default: return "Gray";
                }
            }
        }

        public AirQualityLevel Level { get; } = AirQualityLevel.Unavaible;

        public string LevelString
        {
            get
            {
                switch (Level)
                {
                    case AirQualityLevel.VeryGood: return Resources.AppResources.StringVeryGood;
                    case AirQualityLevel.Good: return Resources.AppResources.StringGood;
                    case AirQualityLevel.Moderate: return Resources.AppResources.StringModerate;
                    case AirQualityLevel.Sufficient: return Resources.AppResources.StringSufficient;
                    case AirQualityLevel.Bad: return Resources.AppResources.StringBad;
                    case AirQualityLevel.VeryBad: return Resources.AppResources.StringVeryBad;
                    default: return Resources.AppResources.StringNotAvailable;
                }
            }
        }

        public double? Value { get; } = null;

        public string ValueString => Value.HasValue ? string.Format("{0:0.0}", Value.Value) : Resources.AppResources.StringNA;

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