using System;
using System.Collections.Generic;
using System.Linq;

namespace SmokSmog.Model
{
    public class AirQualityInfo : IComparable
    {
        private static HashSet<AirQualityInfo> infos = new HashSet<AirQualityInfo>();

        private AirQualityInfo(AirQualityLevel level)
        {
            Level = level;

            switch (Level)
            {
                case AirQualityLevel.VeryGood:
                    Text = Resources.AppResources.StringVeryGood;
                    Color = "#FF00b050";
                    Minimum = 0d;
                    Maximum = 1d;
                    break;

                case AirQualityLevel.Good:
                    Text = Resources.AppResources.StringGood;
                    Color = "#FF92D050";
                    Minimum = 1d;
                    Maximum = 3d;
                    break;

                case AirQualityLevel.Moderate:
                    Text = Resources.AppResources.StringModerate;
                    Color = "#FFFFFF00";
                    Minimum = 3d;
                    Maximum = 5d;
                    break;

                case AirQualityLevel.Sufficient:
                    Text = Resources.AppResources.StringSufficient;
                    Color = "#FFFFC000";
                    Minimum = 5d;
                    Maximum = 7d;
                    break;

                case AirQualityLevel.Bad:
                    Text = Resources.AppResources.StringBad;
                    Color = "#FFFF0000";
                    Minimum = 7d;
                    Maximum = 10d;
                    break;

                case AirQualityLevel.VeryBad:
                    Text = Resources.AppResources.StringVeryBad;
                    Color = "#FFC00000";
                    Minimum = 10d;
                    Maximum = Double.PositiveInfinity;
                    break;

                default:
                    Text = Resources.AppResources.StringNotAvailable;
                    Color = "Gray";
                    Minimum = Double.NegativeInfinity;
                    Maximum = 0;
                    break;
            }
        }

        public string Color { get; }

        public AirQualityLevel Level { get; } = AirQualityLevel.NotAvailable;

        public double Maximum { get; }

        public double Minimum { get; }

        public string Text { get; }

        public static AirQualityInfo Factory(AirQualityLevel level)
        {
            var info = infos.Where(i => i.Level == level).FirstOrDefault();
            if (info == null)
            {
                info = new AirQualityInfo(level);
                infos.Add(info);
            }
            return info;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var aqi = obj as AirQualityInfo;

            if (aqi == null) return 1;
            return Level.CompareTo(aqi.Level);
        }
    }
}