using Windows.UI.Xaml;

namespace SmokSmog.Xaml.Interactivity
{
    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <typeparam name="T">The type the <see cref="Behavior&lt;T&gt;"/> can be attached to.</typeparam>
    /// <remarks>
    /// Behavior is the base class for providing attachable state and commands to an object. The
    /// types the Behavior can be attached to can be controlled by the generic parameter. Override
    /// OnAttached() and OnDetaching() methods to hook and unhook any necessary handlers from the AssociatedObject.
    /// </remarks>
    public abstract class Behavior<T> : Behavior where T : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Behavior&lt;T&gt;"/> class.
        /// </summary>
        protected Behavior()
        {
            _associatedType = typeof(T);
        }

        /// <summary>
        /// Gets the object to which this <see cref="Behavior&lt;T&gt;"/> is attached.
        /// </summary>
        public T Element => _associatedObject as T;

        protected override void OnAttached()
        {
            if (Element != null)
            {
                Element.Loaded += OnElementLoaded;
                Element.Unloaded += OnElementUnLoaded;
            }
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (Element != null)
            {
                Element.Loaded -= OnElementLoaded;
                Element.Unloaded -= OnElementUnLoaded;
            }
            base.OnDetaching();
        }

        protected virtual void OnElementLoaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnElementUnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}