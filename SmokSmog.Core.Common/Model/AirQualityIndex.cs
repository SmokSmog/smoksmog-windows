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
        private double? Value { get; } = null;

        private AirQualityLevel Level { get; } = AirQualityLevel.Unavaible;

        private AirQualityIndex()
        {
        }

        private AirQualityIndex(double? value)
        {
            Value = value;

            if (!Value.HasValue) Level = AirQualityLevel.Unavaible;
            else if (Value <= 1) Level = AirQualityLevel.VeryGood;
            else if (Value <= 3) Level = AirQualityLevel.Good;
            else if (Value <= 5) Level = AirQualityLevel.Moderate;
            else if (Value <= 7) Level = AirQualityLevel.Sufficient;
            else if (Value <= 10) Level = AirQualityLevel.Bad;
            else if (Value > 10) Level = AirQualityLevel.VeryBad;
        }

        public static AirQualityIndex CalculateAirQualityIndex(ParameterType parameterType, double? parameterValue)
        {
            if (!parameterValue.HasValue)
                return new AirQualityIndex();

            double index = parameterValue.Value;
            switch (parameterType)
            {
                case ParameterType.SO2: index /= 350d; break;
                case ParameterType.NO2: index /= 200d; break;
                case ParameterType.CO: index /= 10000d; break;
                case ParameterType.O3: index /= 120d; break;
                case ParameterType.PM10: index /= 100d; break;
                case ParameterType.C6H6: index /= 40d; break;

                case ParameterType.PM25:
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