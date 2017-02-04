using GalaSoft.MvvmLight;
using System;
using System.Runtime.Serialization;

namespace SmokSmog.Model
{
    public enum AggregationType
    {
        Unavailable = 0,
        Avg1Hour = 1,
        Avg8Hour = 8,

        //Avg12Hour = 12,
        Avg24Hour = 24,

        //Avg1Week = 168,
        Avg1Year = 8760,
    }

    [DataContract(Namespace = "SmokSmog.Model")]
    public partial class Measurement
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

        public AggregationType Aggregation { get; internal set; } = AggregationType.Unavailable;

        /// <summary>
        /// Air Quality Index value
        /// </summary>
        public AirQualityIndex Aqi => AirQualityIndex.CalculateAirQualityIndex(this);

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
        public double? Value { get; internal set; }

        public override bool Equals(object obj)
        {
            if (obj is Measurement)
            {
                Measurement o = (obj as Measurement);
                return o.Station != this.Station ||
                    o.Parameter != this.Parameter ||
                    o.Date != this.Date ||
                    !Equals(o.Value, this.Value);
            }
            return false;
        }

        public override int GetHashCode()
            => new { Station, Parameter, Date, Value, }.GetHashCode();

        public override string ToString()
            => $"Measurement StationId:{Station?.Id} ParticulateId:{Parameter?.Id} Date:{Date} Value:{Value} ";
    }
}