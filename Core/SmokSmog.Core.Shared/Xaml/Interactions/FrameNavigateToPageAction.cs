using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SmokSmog.Xaml.Interactions
{
    /// <summary>
    /// An action that switches the current visual to the specified <see cref="Windows.UI.Xaml.Controls.Page"/>.
    /// </summary>
    public sealed class FrameNavigateToPageAction : DependencyObject, IAction
    {
        // Using a DependencyProperty as the backing store for Frame. This enables animation,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(Frame), typeof(FrameNavigateToPageAction), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="Parameter"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter",
            typeof(object),
            typeof(FrameNavigateToPageAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="TargetPage"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty TargetPageProperty = DependencyProperty.Register(
            "TargetPage",
            typeof(string),
            typeof(FrameNavigateToPageAction),
            new PropertyMetadata(null));

        /// <summary> Initializes a new instance of the NavigateToPageAction class. </summary>
        /// IVisualTreeHelper implementation to use when searching the tree for an INavigate target. </param>
        public FrameNavigateToPageAction()
        {
        }

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter which will be passed to the
        /// <see cref="Windows.UI.Xaml.Controls.Frame.Navigate(System.Type,object)"/> method.
        /// </summary>
        public object Parameter
        {
            get { return (object)this.GetValue(FrameNavigateToPageAction.ParameterProperty); }
            set { this.SetValue(FrameNavigateToPageAction.ParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the <see cref="Windows.UI.Xaml.Controls.Page"/>
        /// to navigate to. This is a dependency property.
        /// </summary>
        public string TargetPage
        {
            get { return (string)this.GetValue(FrameNavigateToPageAction.TargetPageProperty); }
            set { this.SetValue(FrameNavigateToPageAction.TargetPageProperty, value); }
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
        public object Execute(object sender, object parameter)
        {
            if (string.IsNullOrEmpty(this.TargetPage))
            {
                return false;
            }

            IXamlMetadataProvider metadataProvider = Application.Current as IXamlMetadataProvider;
            if (metadataProvider == null)
            {
                // This will happen if there are no XAML files in the project other than App.xaml.
                // The markup compiler doesn't bother implementing IXamlMetadataProvider on the app
                // in that case.
                return false;
            }

            IXamlType xamlType = metadataProvider.GetXamlType(this.TargetPage);
            if (xamlType == null)
            {
                return false;
            }

            if (Frame != null)
            {
                if (Frame.Content != xamlType.UnderlyingType)
                    return Frame.Navigate(xamlType.UnderlyingType, this.Parameter ?? parameter);
            }

            return false;
        }
    }
}