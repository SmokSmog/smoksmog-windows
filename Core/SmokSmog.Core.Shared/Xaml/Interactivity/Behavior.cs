using Microsoft.Xaml.Interactivity;
using System;
using System.Reflection;
using Windows.UI.Xaml;

namespace SmokSmog.Xaml.Interactivity
{
    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <remarks>
    /// This is an infrastructure class. Behavior authors should derive from <see
    /// cref="Behavior&lt;T&gt;"/> instead of from this class.
    /// </remarks>
    public abstract class Behavior : DependencyObject, IBehavior
    {
        #region AssociatedObject

        /// <summary>
        /// The backing field for AssociatedObject.
        /// </summary>
        protected internal DependencyObject _associatedObject;

        /// <summary>
        /// Gets the object to which this behavior is attached.
        /// </summary>
        public DependencyObject AssociatedObject => _associatedObject;

        #endregion AssociatedObject

        #region AssociatedType

        /// <summary>
        /// The backing field for AssociatedType property.
        /// </summary>
        protected internal Type _associatedType = typeof(object);

        /// <summary>
        /// The type to which this behavior can be attached.
        /// </summary>
        protected Type AssociatedType => _associatedType;

        #endregion AssociatedType

        #region Attach()

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="dependencyObject">The object to attach to.</param>
        /// <exception cref="InvalidOperationException">
        /// The Behavior is already hosted on a different element.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// dependencyObject does not satisfy the Behavior type constraint.
        /// </exception>
        public void Attach(DependencyObject dependencyObject)
        {
            if (this.AssociatedObject != null)
            {
                throw new InvalidOperationException("The Behavior is already hosted on a different element.");
            }

            _associatedObject = dependencyObject;

            if (dependencyObject != null)
            {
                if (!this.AssociatedType.GetTypeInfo().IsAssignableFrom(dependencyObject.GetType().GetTypeInfo()))
                {
                    throw new InvalidOperationException("dependencyObject does not satisfy the Behavior type constraint.");
                }
                
                OnAttached();
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected virtual void OnAttached()        {        }

        #endregion Attach()

        #region Detach()

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            if (AssociatedObject != null)
            {
                OnDetaching();
                _associatedObject = null;
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has
        /// actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected virtual void OnDetaching() { }

        #endregion Detach()
    }
}