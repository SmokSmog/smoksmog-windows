using System.Collections;
using System.Collections.Generic;

namespace SmokSmog.Linq
{
    public class Grouping<TKey, TElement> : System.Linq.IGrouping<TKey, TElement>
    {
        private TKey _key;

        public TKey Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private bool? _isEmpty = null;

        public bool IsEmpty
        {
            get
            {
                if (!_isEmpty.HasValue)
                    _isEmpty = Items.Count <= 0;
                return _isEmpty.Value;
            }
        }

        private IList<TElement> _items = new List<TElement>();

        public IList<TElement> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                _isEmpty = Items.Count <= 0;
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}