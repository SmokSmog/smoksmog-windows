using GalaSoft.MvvmLight;
using System;
using System.Runtime.Serialization;

namespace SmokSmog.Model
{
    public enum ParameterType
    {
        UNKNOWN = -1,

        SO2 = 1,    // Sulphur dioxide
        NO = 2,     // Nitrogen oxide
        NO2 = 3,    // Nitrogen dioxide
        CO = 4,     // Carbon monoxide
        O3 = 5,     // Ozone
        NOx = 6,    // Nitrogen oxides
        PM10 = 7,   // Particulate matter
        PM25 = 8,   // Particulate matter
        C6H6 = 11,  // Benzene
    }

    //public partial class Parameter : ObservableObject
    //{
    //    //private List<Measurement> _measurements = new List<Measurement>();

    // //public Measurement Current //{ // get // { // var list = Measurements?.Where(o =>
    // o.Aggregation == AggregationType.Avg1Hour).ToArray(); // return list.Any() ? list.MaxBy(o =>
    // o.DateUtc) : new Measurement(Station, this); // } //}

    // //public Measurement CurrentAvg //{ // get // { // var list = Measurements?.Where(o =>
    // o.Aggregation == AggregationType.Avg24Hour).ToArray(); // return list.Any() ? list.MaxBy(o =>
    // o.DateUtc) : new Measurement(Station, this); // } //}

    // /////
    // <summary>
    // ///// example: "2013-10-29 18:15:00" /////
    // </summary>
    // //public DateTime LastSyncDate //{ // get { return LastSyncDateUtc.ToLocalTime(); } //
    // internal set { LastSyncDateUtc = value.ToUniversalTime(); } //}

    // /////
    // <summary>
    // ///// example: 2013-10-29 17:15:00 /////
    // </summary>
    // //public DateTime LastSyncDateUtc { get; internal set; } = DateTime.MinValue;

    //    //public List<Measurement> Measurements
    //    //{
    //    //    get { return _measurements; }
    //    //    set
    //    //    {
    //    //        if (_measurements == value) return;
    //    //        _measurements = value;
    //    //        RaisePropertyChanged(nameof(Measurements));
    //    //    }
    //    //}
    //}

    [DataContract(Namespace = "SmokSmog.Model")]
    public partial class Parameter : ObservableObject
    {
        private string _description = string.Empty;
        private string _name = Resources.AppResources.StringUnknown;
        private Norm _norm = null;
        private string _shortName = Resources.AppResources.StringUnknown;
        private string _unit = Resources.AppResources.StringUnknown;

        public Parameter(Station station, int id)
        {
            Station = station;
            Id = id;
        }

        /// <summary>
        /// default constructor for design purposes only
        /// </summary>
        internal Parameter()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
                throw new NotSupportedException();
        }

        /// <summary>
        /// Several word about parameter
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return _description; }
            internal set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// Identifier
        /// </summary>
        [DataMember]
        public int Id { get; internal set; } = -1;

        /// <summary>
        /// Full name of particulate
        /// example: Carbon Monoxide
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _name; }
            internal set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public Norm Norm
        {
            get { return _norm; }
            set
            {
                if (_norm == value) return;
                _norm = value;
                RaisePropertyChanged(nameof(Norm));
            }
        }

        /// <summary>
        /// Shorter name of particulate (in chemical manner)
        /// example: NOx
        /// example: SO
        /// example: PM₁₀
        /// </summary>
        [DataMember]
        public string ShortName
        {
            get { return _shortName; }
            internal set
            {
                if (_shortName == value) return;
                _shortName = value;
                RaisePropertyChanged(nameof(ShortName));
            }
        }

        public Station Station { get; internal set; }
        public ParameterType Type => Enum.IsDefined(typeof(ParameterType), Id) ? (ParameterType)Id : ParameterType.UNKNOWN;

        /// <summary>
        /// Base unit of particulate in which measurement and norms are presented
        /// example: µg/m3
        /// </summary>
        [DataMember]
        public string Unit
        {
            get { return _unit; }
            internal set
            {
                if (_unit == value) return;
                _unit = value;
                RaisePropertyChanged(nameof(Unit));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Parameter)
            {
                Parameter o = (obj as Parameter);
                return o.Id == this.Id && o.Name.Equals(this.Name);
            }
            return false;
        }

        public override int GetHashCode()
            => new { Id, Name, ShortName, Unit }.GetHashCode();

        public override string ToString()
            => $"{nameof(Parameter)} Id:{Id} Name:{Name} ShortName:{ShortName} Unit:{Unit} Norm:{Norm}";
    }
}