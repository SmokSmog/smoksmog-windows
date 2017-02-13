using SmokSmog.Model;

namespace SmokSmog.Services.Search
{
    public class SearchQuerry
    {
        public enum Type
        {
            Text,
            Geocoordinate,
        }

        public string Text { get; private set; } = null;
        public Geocoordinate Geocoordinate { get; private set; } = null;

        public Type QuerryType { get; }

        public SearchQuerry(string text)
        {
            Text = text;
            QuerryType = Type.Text;
        }

        public SearchQuerry(Geocoordinate geocoordinate)
        {
            Geocoordinate = geocoordinate;
            QuerryType = Type.Geocoordinate;
        }

        public override string ToString()
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(Text))
            {
                result = Text;
            }
            else if (Geocoordinate != null)
            {
                result = Geocoordinate.ToString();
            }
            return result;
        }
    };
}