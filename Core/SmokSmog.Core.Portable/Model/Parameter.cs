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

    [DataContract(Namespace = "SmokSmog.Model")]
    public partial class Parameter : ObservableObject, IComparable<int>
    {
        private string _description = string.Empty;

        private int _id = -1;

        private string _name = Resources.AppResources.StringUnknown;

        private string _normType = Resources.AppResources.StringUnknown;

        private double? _normValue = null;

        private string _shortName = Resources.AppResources.StringUnknown;

        private string _unit = Resources.AppResources.StringUnknown;

        public Parameter(int id) : base()
        {
            _id = id;
        }

        /// <summary>
        /// Several word about parameter
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return _description; }
            set
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
        public int Id
        {
            get { return _id; }
            private set
            {
                if (_id == value) return;
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Full name of particulate
        /// example: Carbon Monoxide
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Norm Type
        /// </summary>
        [DataMember]
        public string NormType
        {
            get { return _normType; }
            set
            {
                if (_normType == value) return;
                _normType = value;
                RaisePropertyChanged(nameof(NormType));
            }
        }

        /// <summary>
        /// The value of the norm expressed in units defined by the field unit
        /// </summary>
        [DataMember]
        public double? NormValue
        {
            get { return _normValue; }
            set
            {
                if (_normValue == value) return;
                _normValue = value;
                RaisePropertyChanged(nameof(NormValue));
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
            set
            {
                if (_shortName == value) return;
                _shortName = value;
                RaisePropertyChanged(nameof(ShortName));
            }
        }

        public ParameterType Type
        {
            get
            {
                if (Enum.IsDefined(typeof(ParameterType), _id))
                    return (ParameterType)_id;
                return ParameterType.UNKNOWN;
            }
        }

        /// <summary>
        /// Base unit of particulate in which measurement and norms are presented
        /// example: µg/m3
        /// </summary>
        [DataMember]
        public string Unit
        {
            get { return _unit; }
            set
            {
                if (_unit == value) return;
                _unit = value;
                RaisePropertyChanged(nameof(Unit));
            }
        }

        public int CompareTo(int integer)
        {
            return integer.CompareTo(this.Id);
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
            => $"{nameof(Parameter)} Id:{Id} Name:{Name} ShortName:{ShortName} Unit:{Unit} NormType:{NormType} Norm:{NormValue}";
    }
}