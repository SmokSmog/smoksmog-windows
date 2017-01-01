using SmokSmog.Model;

namespace SmokSmog.Services.Search
{
    public class SearchQuerry
    {
        public string String;
        public Geocoordinate Geocoordinate;

        public override string ToString()
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(String))
            {
                result = String;
            }
            else if (Geocoordinate != null)
            {
                result = Geocoordinate.ToString();
            }
            return result;
        }
    };
}