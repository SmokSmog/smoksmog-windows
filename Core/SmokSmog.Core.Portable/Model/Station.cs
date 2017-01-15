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

        private int _id = -1;

        private string _name = Resources.AppResources.StringUnknown;

        private string _province = Resources.AppResources.StringUnknown;

        public Station()
        {
        }

        public static Station Empty { get; } = new Station() { Address = string.Empty, City = string.Empty, Name = string.Empty, Province = string.Empty, };

        public static Station Sample { get; } = new Station() { Id = 1, Address = "ul. Bulwarowa", City = "Kraków", Name = "Kraków-Kurdwanów", Province = "Małopolskie", Geocoordinate = new Geocoordinate(50.069308, 20, 053492), };

        /// <summary>
        /// Address - street name, building number etc...
        /// </summary>
        [DataMember]
        public string Address
        {
            get { return _address; }
            set
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
            set
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
            set
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
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Station Name (city) example : "Kraków - Aleja Krasińskiego"
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
        /// Name of Province
        /// </summary>
        [DataMember]
        public string Province
        {
            get { return _province; }
            set
            {
                if (_province == value) return;
                _province = value;
                RaisePropertyChanged(nameof(Province));
            }
        }

        public int CompareTo(int integer)
        {
            return integer.CompareTo(this.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is Station)
            {
                Station o = (obj as Station);
                return o.Id == this.Id &&
                    o.Name.Equals(this.Name) &&
                    o.Geocoordinate == this.Geocoordinate &&
                    o.Province.Equals(this.Province);
            }
            return false;
        }

        public override int GetHashCode()
            => new { Id, Name, Province, Geocoordinate }.GetHashCode();

        public override string ToString()
            => $"Station Id:{Id} Name:{Name} Province:{Province}";
    }
}