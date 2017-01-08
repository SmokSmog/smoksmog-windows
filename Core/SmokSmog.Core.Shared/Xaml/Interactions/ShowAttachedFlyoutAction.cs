using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SmokSmog.Xaml.Interactions
{
    public sealed class ShowAttachedFlyoutAction : DependencyObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="TargetObject"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            "TargetObject",
            typeof(object),
            typeof(ShowAttachedFlyoutAction),
            new PropertyMetadata(null, new PropertyChangedCallback(ShowAttachedFlyoutAction.OnTargetObjectChanged)));

        private Type targetObjectType;

        /// <summary>
        /// Gets or sets the object that exposes the method of interest. This is a dependency property.
        /// </summary>
        public object TargetObject
        {
            get
            {
                return this.GetValue(ShowAttachedFlyoutAction.TargetObjectProperty);
            }

            set
            {
                this.SetValue(ShowAttachedFlyoutAction.TargetObjectProperty, value);
            }
        }

        public object Execute(object sender, object parameter)
        {
            object target;
            if (this.ReadLocalValue(ShowAttachedFlyoutAction.TargetObjectProperty) != DependencyProperty.UnsetValue)
            {
                target = this.TargetObject;
            }
            else
            {
                target = sender;
            }

            var element = target as FrameworkElement;
            if (element != null)
            {
                var flyout = FlyoutBase.GetAttachedFlyout(element);
                if (flyout != null)
                {
                    FlyoutBase.ShowAttachedFlyout(element);
                    return true;
                }
            }
            return false;
        }

        private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ShowAttachedFlyoutAction callMethodAction = (ShowAttachedFlyoutAction)sender;

            Type newType = args.NewValue != null ? args.NewValue.GetType() : null;
            callMethodAction.UpdateTargetType(newType);
        }

        private void UpdateTargetType(Type newTargetType)
        {
            if (newTargetType == this.targetObjectType)
            {
                return;
            }

            this.targetObjectType = newTargetType;
        }
    }
}