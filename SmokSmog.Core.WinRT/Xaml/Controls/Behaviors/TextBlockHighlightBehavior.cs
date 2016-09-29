﻿using SmokSmog.Extensions;
using SmokSmog.Xaml.Controls.Common;
using SmokSmog.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace SmokSmog.Xaml.Controls.Behaviors
{
    /// <summary>
    /// Provides Text highlighting behavior for TextBlock Control for Text or Inline Property
    /// Warring! Text and Inline could not be mixed!!!
    /// </summary>
    public class TextBlockHighlightBehavior : Behavior<TextBlock>
    {
        public TextBlockHighlightBehavior()
        {
            if (DesignMode.DesignModeEnabled)
            {
                UpdateHighlight();
            }
        }

        #region SearchString

        /// <summary>
        /// SearchString Dependency Property
        /// </summary>
        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register(
                "SearchString",
                typeof(string),
                typeof(TextBlockHighlightBehavior),
                new PropertyMetadata(null, OnSearchStringChanged));

        /// <summary>
        /// Gets or sets the SearchString property. This dependency property indicates the search
        /// string to highlight in the associated TextBlock.
        /// </summary>
        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        /// <summary>
        /// Handles changes to the SearchString property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that tracks changes to the effective value of this property.
        /// </param>
        private static void OnSearchStringChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (TextBlockHighlightBehavior)d;
            string oldSearchString = (string)e.OldValue;
            string newSearchString = target.SearchString;
            target.OnSearchStringChanged(oldSearchString, newSearchString);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the SearchString property.
        /// </summary>
        /// <param name="oldSearchString">The old SearchString value</param>
        /// <param name="newSearchString">The new SearchString value</param>
        private void OnSearchStringChanged(
            string oldSearchString, string newSearchString)
        {
            UpdateHighlight();
        }

        #endregion SearchString

        #region IsCaseSensitive

        /// <summary>
        /// IsCaseSensitive Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsCaseSensitiveProperty =
            DependencyProperty.Register(
                "IsCaseSensitive",
                typeof(bool),
                typeof(TextBlockHighlightBehavior),
                new PropertyMetadata(false, OnIsCaseSensitiveChanged));

        /// <summary>
        /// Gets or sets the IsCaseSensitive property. This dependency property indicates whether the
        /// highlight behavior is case sensitive.
        /// </summary>
        public bool IsCaseSensitive
        {
            get { return (bool)GetValue(IsCaseSensitiveProperty); }
            set { SetValue(IsCaseSensitiveProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsCaseSensitive property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that tracks changes to the effective value of this property.
        /// </param>
        private static void OnIsCaseSensitiveChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (TextBlockHighlightBehavior)d;
            bool oldIsCaseSensitive = (bool)e.OldValue;
            bool newIsCaseSensitive = target.IsCaseSensitive;
            target.OnIsCaseSensitiveChanged(oldIsCaseSensitive, newIsCaseSensitive);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsCaseSensitive property.
        /// </summary>
        /// <param name="oldIsCaseSensitive">The old IsCaseSensitive value</param>
        /// <param name="newIsCaseSensitive">The new IsCaseSensitive value</param>
        private void OnIsCaseSensitiveChanged(
            bool oldIsCaseSensitive, bool newIsCaseSensitive)
        {
            UpdateHighlight();
        }

        #endregion IsCaseSensitive

        #region Highlight

        /// <summary>
        /// HighlightBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty HighlightProperty =
            DependencyProperty.Register(
                "Highlight",
                typeof(Brush),
                typeof(TextBlockHighlightBehavior),
                new PropertyMetadata(new SolidColorBrush(Colors.Red), OnHighlightChanged));

        /// <summary>
        /// Gets or sets the HighlightBrush property. This dependency property indicates the brush to
        /// use to highlight the found instances of the search string.
        /// </summary>
        /// <remarks>Note that the brush is ignored if HighlightTemplate is specified</remarks>
        public Brush Highlight
        {
            get { return (Brush)GetValue(HighlightProperty); }
            set { SetValue(HighlightProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HighlightBrush property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that tracks changes to the effective value of this property.
        /// </param>
        private static void OnHighlightChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (TextBlockHighlightBehavior)d;
            Brush oldHighlightBrush = (Brush)e.OldValue;
            Brush newHighlightBrush = target.Highlight;
            target.OnHighlightChanged(oldHighlightBrush, newHighlightBrush);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the HighlightBrush property.
        /// </summary>
        /// <param name="oldHighlightBrush">The old HighlightBrush value</param>
        /// <param name="newHighlightBrush">The new HighlightBrush value</param>
        private void OnHighlightChanged(
            Brush oldHighlightBrush, Brush newHighlightBrush)
        {
            UpdateHighlight();
        }

        #endregion Highlight

        private PropertyChangeEventSource<string> _textChangeEventSource;
        private string _originalText = string.Empty;
        private ObservableCollection<Inline> _originalInlineList = new ObservableCollection<Inline>();
        private List<PropertyChangeEventSource<string>> _runTextChangeEventSourceList = new List<PropertyChangeEventSource<string>>();

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            _textChangeEventSource = new PropertyChangeEventSource<string>(this.AssociatedObject, "Text", BindingMode.OneWay);

            // preserve original InlineCollection of TextBlock
            foreach (var inline in this.AssociatedObject.Inlines)
            {
                _runTextChangeEventSourceList.Add(new PropertyChangeEventSource<string>(inline, "Text", BindingMode.OneWay));
                _originalInlineList.Add(inline);
            }

            foreach (var inline in _runTextChangeEventSourceList)
            {
                inline.ValueChanged += InlineChanged;
            }
            _textChangeEventSource.ValueChanged += TextChanged;

            UpdateHighlight();
            base.OnAttached();
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has
        /// actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            ClearHighlight();
            _textChangeEventSource.ValueChanged -= TextChanged;
            _textChangeEventSource = null;

            foreach (var item in _runTextChangeEventSourceList)
            {
                item.ValueChanged -= InlineChanged;
            }
            _runTextChangeEventSourceList.Clear();
            _originalInlineList.Clear();
            base.OnDetaching();
        }

        private void TextChanged(object sender, string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                ClearHighlight();
                return;
            }
            _originalText = s;
            UpdateHighlight();
        }

        private void InlineChanged(object sender, string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                ClearHighlight();
                return;
            }

            StringBuilder textStringBuilder = new StringBuilder();
            foreach (var inline in _originalInlineList)
            {
                if (inline is LineBreak)
                {
                    textStringBuilder.Append(Environment.NewLine);
                }
                else if (inline is Run)
                {
                    Run texta = (Run)inline;
                    textStringBuilder.Append(texta.Text);
                }
            }
            _originalText = textStringBuilder.ToString();

            UpdateHighlight();
        }

        /// <summary>
        /// Updates the highlight.
        /// </summary>
        public void UpdateHighlight()
        {
            if (this.AssociatedObject == null || string.IsNullOrEmpty(this.SearchString))
            {
                ClearHighlight();
                return;
            }

            string text = _originalText;

            int len = SearchString.Length;
            AssociatedObject.Inlines.Clear();

            if (string.IsNullOrWhiteSpace(SearchString))
            {
                AssociatedObject.Inlines.Add(new Run() { Text = text });
            }
            else
            {
                //var indexesList = Text.IndexOfAll(TextHighlighted);
                int last = 0;

                var exp = SearchString.Split(' ');
                var indexOfAll = text.IndexOfAll(exp, IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);

                var type = new { start = 0, end = 0 };
                // create list of annonymus type
                var indList = new[] { type }.ToList(); indList.RemoveAt(0);

                foreach (var item in indexOfAll)
                {
                    foreach (var index in item.Value) { indList.Add(new { start = index, end = index + item.Key.Length }); }
                }
                //sort by indexes
                indList.Sort((a, b) => a.start.CompareTo(b.start));
                // create list of annonymus type
                var indListAgregated = new[] { type }.ToList(); indListAgregated.RemoveAt(0);

                foreach (var item in indList)
                {
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
                    int l = item.start - last;
                    if (item.start != last && l > 0)
                    {
                        AssociatedObject.Inlines.Add(new Run() { Text = text.Substring(last, l) });
                    }
                    AssociatedObject.Inlines.Add(new Run() { Text = text.Substring(item.start, item.end - item.start), Foreground = Highlight });
                    last = item.end;
                }

                if (last < text.Length)
                {
                    var l = text.Length - last;
                    AssociatedObject.Inlines.Add(new Run() { Text = text.Substring(last, l) });
                }
            }
        }

        /// <summary>
        /// Clears the highlight.
        /// </summary>
        public void ClearHighlight()
        {
            if (this.AssociatedObject == null)
            {
                return;
            }

            this.AssociatedObject.Inlines.Clear();
            this.AssociatedObject.Inlines.Add(new Run() { Text = _originalText });
        }
    }
}