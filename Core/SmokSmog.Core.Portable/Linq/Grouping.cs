using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace SmokSmog.Linq
{
    public class Grouping<TKey, TElement> : ObservableObject, System.Linq.IGrouping<TKey, TElement>
    {
        private ObservableCollection<TElement> _items = new ObservableCollection<TElement>();

        private TKey _key = default(TKey);

        public Grouping(TKey key)
        {
            Key = key;
        }

        public Grouping(TKey key, IEnumerable<TElement> items)
            : this(key)
        {
            Items = new ObservableCollection<TElement>(items);
        }

        public bool IsEmpty => !Items?.Any() ?? true;

        public ObservableCollection<TElement> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public TKey Key
        {
            get { return _key; }
            set
            {
                if (_key != null && _key.Equals(value)) return;
                _key = value;
                RaisePropertyChanged(nameof(Key));
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Items?.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items?.GetEnumerator();
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}