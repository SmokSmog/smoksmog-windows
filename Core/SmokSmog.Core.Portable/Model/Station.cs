using GalaSoft.MvvmLight;
using System;
using System.Runtime.Serialization;

namespace SmokSmog.Model
{
    [DataContract(Namespace = "SmokSmog.Model")]
    public partial class Station : ObservableObject, IComparable<int>
    {
        private string _address = Resources.AppResources.StringUnknown;
        private string _city = Resources.AppResources.StringUnknown;
        private Geocoordinate _geocoordinate = new Geocoordinate();
        private string _name = Resources.AppResources.StringUnknown;
        private string _province = Resources.AppResources.StringUnknown;

        /// <summary>
        /// default constructor for design purposes only
        /// </summary>
        internal Station()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                throw new NotSupportedException();
            }
        }

        public Station(int id)
        {
            Id = id;
        }

        public static Station Sample { get; } = new Station(1)
        {
            Address = "ul. Bulwarowa",
            City = "Kraków",
            Name = "Kraków-Kurdwanów",
            Province = "Małopolskie",
            Geocoordinate = new Geocoordinate(50.069308, 20.053492),
        };

        /// <summary>
        /// Address - street name, building number etc...
        /// </summary>
        [DataMember]
        public string Address
        {
            get { return _address; }
            internal set
            {
                if (_address == value) return;
                _address = value;
                RaisePropertyChanged(nameof(Address));
            }
        }

        /// <summary>
        /// Name of City
        /// </summary>
        [DataMember]
        public string City
        {
            get { return _city; }
            internal set
            {
                if (_city == value) return;
                _city = value;
                RaisePropertyChanged(nameof(City));
            }
        }

        /// <summary>
        /// Station GPS position
        /// </summary>
        [DataMember]
        public Geocoordinate Geocoordinate
        {
            get { return _geocoordinate; }
            internal set
            {
                if (_geocoordinate == value) return;
                _geocoordinate = value;
                RaisePropertyChanged(nameof(Geocoordinate));
            }
        }

        /// <summary>
        /// Station identifier example : 1 it must be unique
        /// </summary>
        [DataMember]
        public int Id { get; internal set; } = -1;

        /// <summary>
        /// Station Name (city) example : "Kraków - Aleja Krasińskiego"
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

        /// <summary>
        /// Name of Province
        /// </summary>
        [DataMember]
        public string Province
        {
            get { return _province; }
            internal set
            {
                if (_province == value) return;
                _province = value;
                RaisePropertyChanged(nameof(Province));
            }
        }

        internal static Station Empty { get; } = new Station(-1)
        {
            Address = string.Empty,
            City = string.Empty,
            Name = string.Empty,
            Province = string.Empty,
        };

        public int CompareTo(int integer)
        {
            return integer.CompareTo(this.Id);
        }

        public override bool Equals(object obj)
        {
            var station = obj as Station;
            if (station != null)
            {
                return station.Id == this.Id &&
                    station.Name.Equals(this.Name) &&
                    station.Geocoordinate == this.Geocoordinate &&
                    station.Province.Equals(this.Province);
            }
            return false;
        }

        public override int GetHashCode()
            => new { Id, Name, Province, Geocoordinate }.GetHashCode();

        public override string ToString()
            => $"{nameof(Station)} Id:{Id} Name:{Name} Province:{Province}";
    }
}