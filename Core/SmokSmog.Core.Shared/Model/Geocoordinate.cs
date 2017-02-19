using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace SmokSmog.Model
{
    [DataContract(Namespace = "SmokSmog.Model")]
    public class Geocoordinate : ObservableObject, IEquatable<Geocoordinate>
    {
        private double _accuracy = double.MaxValue;

        private double? _altitude = null;

        private double _latitude = 0.0d;

        private double _longitude = 0.0d;

        private double? _speed = null;

        public Geocoordinate()
        {
        }

        public Geocoordinate(double latitude, double longitude, double accuracy = 0d)
        {
            _latitude = latitude;
            _longitude = longitude;
            _accuracy = accuracy;
        }

        public enum DistanceType { Miles, Kilometers, Meters };

        /// <summary>
        /// Coordinate accuracy
        /// </summary>
        [DataMember]
        public double Accuracy
        {
            get { return _accuracy; }
            set
            {
                if (_accuracy == value) return;
                _accuracy = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Absolute altitude
        /// </summary>
        [DataMember]
        public double? Altitude
        {
            get { return _altitude; }
            set
            {
                if (_altitude == value) return;
                _altitude = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Latitude example : "50.056805"
        /// </summary>
        [DataMember]
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude == value) return;
                _latitude = value; RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Longitude example : "19.926426"
        /// </summary>
        [DataMember]
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude == value) return;
                _longitude = value; RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Current speed
        /// </summary>
        [DataMember]
        public double? Speed
        {
            get { return _speed; }
            set
            {
                if (_speed == value) return;
                _speed = value; RaisePropertyChanged();
            }
        }

        public static bool operator !=(Geocoordinate left, Geocoordinate right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(Geocoordinate left, Geocoordinate right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Distance calculated using Haversine formula
        /// </summary>
        /// <param name="geocoordinate"></param>
        /// <param name="type"></param>
        /// <seealso cref="http://stackoverflow.com/questions/28569246/how-to-get-distance-between-two-locations-in-windows-phone-8-1"/>
        /// <seealso cref="https://en.wikipedia.org/wiki/Haversine_formula"/>
        /// <returns></returns>
        public double Distance(Geocoordinate geocoordinate, DistanceType type)
        {
            if (double.IsNaN(this.Latitude) || double.IsNaN(this.Longitude) ||
                double.IsNaN(geocoordinate.Latitude) || double.IsNaN(geocoordinate.Longitude))
            {
                throw new ArgumentException("Argument_LatitudeOrLongitudeIsNotANumber");
            }

            Func<double, double> toRadian = degree => Math.PI / 180 * degree;

            double r = 6371d;

            switch (type)
            {
                case DistanceType.Miles:
                    r = 3960d;
                    break;

                case DistanceType.Kilometers:
                    r = 6371d;
                    break;

                case DistanceType.Meters:
                    r = 6371000d;
                    break;
            }

            double dLat = toRadian(geocoordinate.Latitude - this.Latitude);
            double dLon = toRadian(geocoordinate.Longitude - this.Longitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(toRadian(this.Latitude)) * Math.Cos(toRadian(geocoordinate.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = r * c;
            return d;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Geocoordinate)obj);
        }

        public bool Equals(Geocoordinate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.Accuracy == other.Accuracy &&
                   this.Latitude == other.Latitude &&
                   this.Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 41;

                hash = hash * 59 + this.Accuracy.GetHashCode();

                if (this.Altitude.HasValue)
                    hash = hash * 59 + this.Altitude.GetHashCode();

                hash = hash * 59 + this.Latitude.GetHashCode();

                hash = hash * 59 + this.Longitude.GetHashCode();

                if (this.Speed.HasValue)
                    hash = hash * 59 + this.Speed.GetHashCode();

                return hash;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public override string ToString()
        {
            return $"Geo coordinate Latitude:{Longitude} Longitude:{Latitude} Altitude:{Altitude}";
        }
    }
}