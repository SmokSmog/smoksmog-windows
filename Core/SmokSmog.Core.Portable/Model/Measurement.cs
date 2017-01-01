using System;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight;

namespace SmokSmog.Model
{
    public enum MeasurementAggregationType
    {
        Avg1Hour = 1,
        Avg8Hour = 8,
        Avg12Hour = 12,
        Avg1Day = 24,
        Avg1Week = 168,
        Avg1Year = 8760,
    }

    [DataContract(Namespace = "SmokSmog.Model")]
    public partial class Measurement : ObservableObject
    {
        private DateTime _date = DateTime.MinValue;

        private int _parameterId = -1;

        private TimeSpan _period;

        private int _stationId;

        private double? _value;

        public Measurement(int stationId, int parameterId)
        {
            _stationId = stationId;
            _parameterId = parameterId;
        }

        /// <summary>
        /// Air Quality Index value
        /// </summary>
        public AirQualityIndex Aqi => AirQualityIndex.CalculateAirQualityIndex((ParameterType)_parameterId, Value);

        /// <summary>
        /// Date and time of Measurement
        /// example: "2013-10-29 18:15:00"
        /// </summary>
        [DataMember]
        public DateTime Date
        {
            get { return _date; }
            set
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
        /// Identification number of Parameter to which this measurement belongs
        /// </summary>
        [DataMember]
        public int ParameterId
        {
            get { return _parameterId; }
            private set
            {
                if (_parameterId == value) return;
                _parameterId = value;
                RaisePropertyChanged(nameof(ParameterId));
            }
        }

        /// <summary>
        /// Period from which the values are averaged It determine frequency of date as well
        /// </summary>
        [DataMember]
        public TimeSpan Period
        {
            get { return _period; }
            set
            {
                if (_period == value) return;
                _period = value;
                RaisePropertyChanged(nameof(Period));
            }
        }

        /// <summary>
        /// Station Id
        /// </summary>
        [DataMember]
        public int StationId
        {
            get { return _stationId; }
            private set
            {
                if (_stationId == value) return;
                _stationId = value;
                RaisePropertyChanged(nameof(StationId));
            }
        }

        /// <summary>
        /// Value
        /// example: "23.3"
        /// </summary>
        [DataMember]
        public double? Value
        {
            get { return _value; }
            set
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
                return o.StationId == this.StationId &&
                    o.ParameterId == this.ParameterId &&
                    o.Date == this.Date &&
                    o.Value == this.Value &&
                    o.Period == this.Period;
            }
            return false;
        }

        public override int GetHashCode()
            => new { StationId, ParameterId, Date, Value, Period }.GetHashCode();

        public override string ToString()
            => $"Measurement StationId:{StationId} ParticulateId:{ParameterId} Date:{Date} Value:{Value} Period:{Period}";
    }
}