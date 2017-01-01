using SmokSmog.Extensions;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace SmokSmog.Xaml.Controls
{
    public sealed partial class TextBlockHighlighted : UserControl /*, Behavior<TextBlock>*/
    {
        public TextBlockHighlighted()
        {
            this.InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text. This enables animation, styling,
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBlockHighlighted),
                 new PropertyMetadata("", new PropertyChangedCallback(OnSomethingChanged)));

        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextHighlighted. This enables
        // animation, styling, binding, etc...
        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register("SearchString", typeof(string), typeof(TextBlockHighlighted),
                new PropertyMetadata("", new PropertyChangedCallback(OnSomethingChanged)));

        public Brush Highlight
        {
            get { return (Brush)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Highlight. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty HighlightProperty =
            DependencyProperty.Register("Highlight", typeof(Brush), typeof(TextBlockHighlighted),
                new PropertyMetadata(new SolidColorBrush(Colors.Blue), new PropertyChangedCallback(OnSomethingChanged)));

        private object _content = new object();

        public new object Content
        {
            get { return _content; }
            set { _content = value; SetValue(InlinesProperty, new Documents.InlineCollection() { new Run() { Text = value.ToString() } }); }
        }

        public Documents.InlineCollection Inlines
        {
            get { return (Documents.InlineCollection)GetValue(InlinesProperty); }
            private set { SetValue(InlinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Inlines. This enables animation,
        // styling, binding, etc...
        public static readonly DependencyProperty InlinesProperty =
            DependencyProperty.Register("Inlines", typeof(Documents.InlineCollection), typeof(TextBlockHighlighted),
                new PropertyMetadata(new Documents.InlineCollection(), new PropertyChangedCallback(OnSomethingChanged)));

        public static void OnSomethingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            TextBlockHighlighted tbh = obj as TextBlockHighlighted;

            if (tbh == null)
                return;

            var cts = tbh._lastProcessCTS;
            if (cts != null)
            {
                if (!cts.IsCancellationRequested)
                    cts.Cancel();
            }
            cts = new CancellationTokenSource();
            tbh._lastProcessCTS = cts;

            tbh.Process(cts.Token);
        }

        private CancellationTokenSource _lastProcessCTS = null;

        private async void Process(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            StringBuilder stringBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.Text))
            {
                stringBuilder.Append(this.Text);
            }
            else
            {
                foreach (var inline in this.Inlines)
                {
                    if (token.IsCancellationRequested) return;
                    stringBuilder.Append(inline.ContentStart);
                }
            }
            string text = stringBuilder.ToString();

            int len = SearchString.Length;

            TextBlock backStoreTextBlock = new TextBlock();

            if (token.IsCancellationRequested) return;
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                backStoreTextBlock.Inlines.Add(new Run() { Text = text });
            }
            else
            {
                //var indexesList = Text.IndexOfAll(TextHighlighted);
                int last = 0;

                var exp = SearchString.Split(' ');
                var indexOfAll = text.IndexOfAll(exp, System.StringComparison.OrdinalIgnoreCase);

                var type = new { start = 0, end = 0 };
                // create list of annonymus type
                var indList = new[] { type }.ToList(); indList.RemoveAt(0);

                foreach (var item in indexOfAll)
                {
                    if (token.IsCancellationRequested) return;
                    foreach (var index in item.Value) { indList.Add(new { start = index, end = index + item.Key.Length }); }
                }
                //sort by indexes
                indList.Sort((a, b) => a.start.CompareTo(b.start));

                if (token.IsCancellationRequested) return;

                // create list of annonymus type
                var indListAgregated = new[] { type }.ToList(); indListAgregated.RemoveAt(0);

                foreach (var item in indList)
                {
                    if (token.IsCancellationRequested) return;

                    if (indListAgregated.Count == 0) { indListAgregated.Add(item); continue; }
                    else
                    {
                        var elem = indListAgregated[indListAgregated.Count - 1];
                        // skip
                        if (elem.start <= item.start && elem.end >= item.end) continue;
                        //merge
                        if (elem.start <= item.start && elem.end < item.end && elem.end >= item.start)
                        {
                            elem = new { start = elem.start, end = item.end };
                            indListAgregated[indListAgregated.Count - 1] = elem;
                        }
                        //create new
                        if (elem.end < item.start) { indListAgregated.Add(item); }
                    }
                }

                foreach (var item in indListAgregated)
                {
                    if (token.IsCancellationRequested) return;

                    int l = item.start - last;
                    if (item.start != last && l > 0)
                    {
                        backStoreTextBlock.Inlines.Add(new Run() { Text = text.Substring(last, l) });
                    }
                    backStoreTextBlock.Inlines.Add(new Run() { Text = text.Substring(item.start, item.end - item.start), Foreground = Highlight });
                    last = item.end;
                }

                if (last < text.Length)
                {
                    var l = text.Length - last;
                    backStoreTextBlock.Inlines.Add(new Run() { Text = text.Substring(last, l) });
                }
            }

            tb.Inlines.Clear();
            foreach (var inline in backStoreTextBlock.Inlines)
            {
                tb.Inlines.Add(inline);
            }

            await Task.Delay(System.TimeSpan.FromSeconds(0));
        }
    }
}