using System;
using System.Collections.Generic;
using System.Linq;

namespace SmokSmog.Model
{
    using Resources;

    public class AirQualityInfo : IComparable
    {
        private static readonly HashSet<AirQualityInfo> Infos = new HashSet<AirQualityInfo>();

        private AirQualityInfo(AirQualityLevel level)
        {
            Level = level;

            switch (Level)
            {
                case AirQualityLevel.VeryGood:
                    Text = LocalizedStrings.GetString("VeryGood");
                    Color = "#FF00b050";
                    Minimum = 0d;
                    Maximum = 1d;
                    break;

                case AirQualityLevel.Good:
                    Text = LocalizedStrings.GetString("Good");
                    Color = "#FF92D050";
                    Minimum = 1d;
                    Maximum = 3d;
                    break;

                case AirQualityLevel.Moderate:
                    Text = LocalizedStrings.GetString("Moderate");
                    Color = "#FFFFFF00";
                    Minimum = 3d;
                    Maximum = 5d;
                    break;

                case AirQualityLevel.Sufficient:
                    Text = LocalizedStrings.GetString("Sufficient");
                    Color = "#FFFFC000";
                    Minimum = 5d;
                    Maximum = 7d;
                    break;

                case AirQualityLevel.Bad:
                    Text = LocalizedStrings.GetString("Bad");
                    Color = "#FFFF0000";
                    Minimum = 7d;
                    Maximum = 10d;
                    break;

                case AirQualityLevel.VeryBad:
                    Text = LocalizedStrings.GetString("VeryBad");
                    Color = "#FFC00000";
                    Minimum = 10d;
                    Maximum = Double.PositiveInfinity;
                    break;

                default:
                    Text = LocalizedStrings.GetString("NotAvailable");
                    Color = "Gray";
                    Minimum = Double.NegativeInfinity;
                    Maximum = 0;
                    break;
            }
        }

        public string Color { get; }

        public AirQualityLevel Level { get; }

        public double Maximum { get; }

        public double Minimum { get; }

        public string Text { get; }

        public static AirQualityInfo Factory(AirQualityLevel level)
        {
            var info = Infos.FirstOrDefault(i => i.Level == level);
            if (info == null)
            {
                info = new AirQualityInfo(level);
                Infos.Add(info);
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