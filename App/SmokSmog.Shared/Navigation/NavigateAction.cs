using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace SmokSmog.Navigation
{
    /// <summary>
    /// An action that switches the current visual to the specified <see cref="Windows.UI.Xaml.Controls.Page"/>.
    /// </summary>
    public class NavigateAction : DependencyObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="Parameter"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter",
            typeof(object),
            typeof(NavigateAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="TargetPage"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TargetPageProperty = DependencyProperty.Register(
            "TargetPage",
            typeof(string),
            typeof(NavigateAction),
            new PropertyMetadata(null));

        /// <summary> Initializes a new instance of the NavigateToPageAction class. </summary>
        /// IVisualTreeHelper implementation to use when searching the tree for an INavigate target. </param>
        public NavigateAction()
        {
        }

        /// <summary>
        /// Gets or sets the parameter which will be passed to the
        /// <see cref="Windows.UI.Xaml.Controls.Frame.Navigate(System.Type,object)"/> method.
        /// </summary>
        public object Parameter
        {
            get { return (object)this.GetValue(NavigateAction.ParameterProperty); }
            set { this.SetValue(NavigateAction.ParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the <see cref="Windows.UI.Xaml.Controls.Page"/>
        /// to navigate to. This is a dependency property.
        /// </summary>
        public string TargetPage
        {
            get { return (string)this.GetValue(NavigateAction.TargetPageProperty); }
            set { this.SetValue(NavigateAction.TargetPageProperty, value); }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">   
        /// The <see cref="System.Object"/> that is passed to the action by the behavior. Generally
        /// this is <seealso cref="Microsoft.Xaml.Interactivity.IBehavior.AssociatedObject"/> or a
        /// target object.
        /// </param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the navigation to the specified page is successful; else false.</returns>
        public virtual object Execute(object sender, object parameter)
        {
            var navProvider = Application.Current as INavigationProvider;
            navProvider?.NavigationService?.NavigateTo(TargetPage, Parameter);
            return false;
        }
    }
}