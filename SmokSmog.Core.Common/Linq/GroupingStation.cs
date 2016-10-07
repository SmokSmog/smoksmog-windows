using System.Collections.Generic;

namespace SmokSmog.Linq
{
    public class GroupingStation : Grouping<string, Model.Station>
    {
        public GroupingStation(string key)
            : base(key)
        {
        }

        public GroupingStation(string key, IEnumerable<Model.Station> items)
            : base(key, items)
        {
        }
    }
}