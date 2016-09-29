using GalaSoft.MvvmLight;
using System;
using System.Runtime.Serialization;

namespace SmokSmog.Model
{
    [DataContract(Namespace = "SmokSmog.Model")]
    public class StationState : ObservableObject
    {
        private double? _aqi = null;

        private bool _isActive = false;

        private DateTime _lastCheckUtc = DateTime.MinValue;
        private DateTime _lastUpdateUtc = DateTime.MinValue;
        private int[] _ParametersIds = new int[] { };
        private int _stationId = -1;

        public StationState()
        {
            _lastCheckUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Air Quality Index Numerical Value
        /// </summary>
        [DataMember]
        public double? Aqi
        {
            get { return _aqi; }
            set
            {
                if (_aqi == value) return;
                _aqi = value;
                RaisePropertyChanged(nameof(Aqi));
            }
        }

        /// <summary>
        /// Determine if station is active
        /// </summary>
        [DataMember]
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;
                _isActive = value;
                RaisePropertyChanged(nameof(IsActive));
            }
        }

        /// <summary>
        /// Date and Time when data was downloaded
        /// </summary>
        [DataMember]
        public DateTime LastCheckUtc
        {
            get { return _lastCheckUtc; }
            set
            {
                if (_lastCheckUtc == value) return;
                _lastCheckUtc = value;
                RaisePropertyChanged(nameof(LastCheckUtc));
            }
        }

        /// <summary>
        /// Date and Time when data was updated on server
        /// </summary>
        [DataMember]
        public DateTime LastUpdateUtc
        {
            get { return _lastUpdateUtc; }
            set
            {
                if (_lastUpdateUtc == value) return;
                _lastUpdateUtc = value;
                RaisePropertyChanged(nameof(LastUpdateUtc));
            }
        }

        /// <summary>
        /// Array with supported parameters ids
        /// </summary>
        [DataMember]
        public int[] ParametersIds
        {
            get { return _ParametersIds; }
            set
            {
                if (_ParametersIds == value) return;
                _ParametersIds = value;
                RaisePropertyChanged(nameof(ParametersIds));
            }
        }

        /// <summary>
        /// Id of station with info is related
        /// </summary>
        [DataMember]
        public int StationId
        {
            get { return _stationId; }
            set
            {
                if (_stationId == value) return;
                _stationId = value;
                RaisePropertyChanged(nameof(StationId));
            }
        }
    }
}