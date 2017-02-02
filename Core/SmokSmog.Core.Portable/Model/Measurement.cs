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
        Avg12Hour = 12,
        Avg24Hour = 24,
        Avg1Week = 168,
        Avg1Year = 8760,
    }

    public struct Average
    {
        public Average(AggregationType aggregationType, double value)
        {
            Value = value;
            AggregationType = aggregationType;
        }

        public double? Value { get; internal set; }
        public AggregationType AggregationType { get; internal set; }
    }

    [DataContract(Namespace = "SmokSmog.Model")]
    public partial class Measurement : ObservableObject
    {
        private Average _average;
        private DateTime _date = DateTime.MinValue;
        private TimeSpan _period;
        private double? _value;

        /// <summary>
        /// default constructor for design purposes only
        /// </summary>
        internal Measurement()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                throw new NotSupportedException();
            }
        }

        public Measurement(Station station, Parameter parameter)
        {
            Station = station;
            Parameter = parameter;
        }

        /// <summary>
        /// Air Quality Index value
        /// </summary>
        public AirQualityIndex Aqi => AirQualityIndex.CalculateAirQualityIndex(Parameter.Type, Value);

        public Average Average
        {
            get { return _average; }
            internal set
            {
                _average = value;
                RaisePropertyChanged(nameof(Average));
            }
        }

        /// <summary>
        /// Date and time of Measurement
        /// example: "2013-10-29 18:15:00"
        /// </summary>
        [DataMember]
        public DateTime Date
        {
            get { return _date; }
            internal set
            {
                if (_date == value) return;
                _date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        /// <summary>
        /// Date and time of Measurement in UTC time
        /// example: 2013-10-29 17:15:00
        /// </summary>
        public DateTime DateUTC => Date.ToUniversalTime();

        /// <summary>
        /// Parameter to which this measurement belongs
        /// </summary>
        public Parameter Parameter { get; }

        /// <summary>
        /// Period from which the values are averaged It determine frequency of date as well
        /// </summary>
        [DataMember]
        public TimeSpan Period
        {
            get { return _period; }
            internal set
            {
                if (_period == value) return;
                _period = value;
                RaisePropertyChanged(nameof(Period));
            }
        }

        /// <summary>
        /// Station to which this measurement belongs
        /// </summary>
        public Station Station { get; }

        /// <summary>
        /// Value
        /// example: "23.3"
        /// </summary>
        [DataMember]
        public double? Value
        {
            get { return _value; }
            internal set
            {
                if (_value == value) return;
                _value = value;
                RaisePropertyChanged(nameof(Value));
                RaisePropertyChanged(nameof(Aqi));
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
                    !Equals(o.Value, this.Value) ||
                    o.Period != this.Period;
            }
            return false;
        }

        public override int GetHashCode()
            => new { Station, Parameter, Date, Value, Period }.GetHashCode();

        public override string ToString()
            => $"Measurement StationId:{Station?.Id} ParticulateId:{Parameter?.Id} Date:{Date} Value:{Value} Period:{Period}";
    }
}