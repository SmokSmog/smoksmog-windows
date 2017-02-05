using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmokSmog.Model
{
    public enum AggregationType
    {
        Avg1Hour = 1,
        Avg8Hour = 8,
        Avg24Hour = 24,
        Avg1Year = 8760,
    }

    public class Measurement
    {
        public Measurement(Station station, Parameter parameter)
        {
            Station = station;
            Parameter = parameter;
        }

        /// <summary>
        /// default constructor for design purposes only
        /// </summary>
        internal Measurement()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
                throw new NotSupportedException();
        }

        /// <summary>
        /// Air Quality Index value
        /// </summary>
        public AirQualityIndex Aqi => AirQualityIndex.CalculateAirQualityIndex(this);

        public double? Avg1Hour
        {
            get { return Values[AggregationType.Avg1Hour]; }
            set { Values[AggregationType.Avg1Hour] = value; }
        }

        public double? Avg1Year
        {
            get { return Values[AggregationType.Avg1Year]; }
            set { Values[AggregationType.Avg1Year] = value; }
        }

        public double? Avg24Hour
        {
            get { return Values[AggregationType.Avg24Hour]; }
            set { Values[AggregationType.Avg24Hour] = value; }
        }

        public double? Avg8Hour
        {
            get { return Values[AggregationType.Avg8Hour]; }
            set { Values[AggregationType.Avg8Hour] = value; }
        }

        /// <summary>
        /// Date and time of Measurement
        /// example: "2013-10-29 18:15:00"
        /// </summary>
        public DateTime Date
        {
            get { return DateUtc.ToLocalTime(); }
            internal set { DateUtc = value.ToUniversalTime(); }
        }

        /// <summary>
        /// Date and time of Measurement in UTC time
        /// example: 2013-10-29 17:15:00
        /// </summary>
        public DateTime DateUtc { get; internal set; } = DateTime.MinValue;

        /// <summary>
        /// Parameter to which this measurement belongs
        /// </summary>
        public Parameter Parameter { get; }

        /// <summary>
        /// Station to which this measurement belongs
        /// </summary>
        public Station Station { get; }

        /// <summary>
        /// Value
        /// example: "23.3"
        /// </summary>

        public Dictionary<AggregationType, double?> Values { get; } = new Dictionary
            <AggregationType, double?>
            {
                    { AggregationType.Avg1Hour , null },
                    { AggregationType.Avg8Hour , null },
                    { AggregationType.Avg24Hour, null },
                    { AggregationType.Avg1Year , null },
            };

        public double? this[string key]
        {
            get
            {
                AggregationType type;
                double? value = null;
                if (Enum.TryParse(key, out type))
                    Values.TryGetValue(type, out value);

                return value;
            }
        }

        public double? this[AggregationType type]
        {
            get
            {
                double? value = null;
                Values.TryGetValue(type, out value);
                return value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Measurement)
            {
                Measurement o = (obj as Measurement);
                return o.Station != this.Station ||
                    o.Parameter != this.Parameter ||
                    o.Date != this.Date ||
                    !Equals(o.Values, this.Values);
            }
            return false;
        }

        public override int GetHashCode()
            => new { Station, Parameter, Date, Values, }.GetHashCode();

        public override string ToString()
            => $"Measurement StationId:{Station?.Id} ParticulateId:{Parameter?.Id} Date:{Date} Values:{Values} ";
    }

    public class Measurements : ObservableCollection<Measurement> { }
}