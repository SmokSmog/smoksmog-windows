using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SmokSmog.Navigation
{
    public class NavigationService : INavigationService
    {
        public const string RootPageKey = "-- ROOT --";
        public const string UnknownPageKey = "-- UNKNOWN --";

        public NavigationService()
        {
        }

        public string CurrentPageKey { get; }

        public string CurrentSecondPageKey { get; }

        //public void Configure(string key, Type pageType);

        public void GoBack()
        {
        }

        public void NavigateTo(string pageKey)
            => NavigateTo(pageKey, null);

        public void NavigateTo(string pageKey, object parameter)
        {
            if (string.IsNullOrEmpty(pageKey))
                return;

            IXamlMetadataProvider metadataProvider = Application.Current as IXamlMetadataProvider;
            if (metadataProvider == null)
            {
                // This will happen if there are no XAML files in the project other than App.xaml.
                // The markup compiler doesn't bother implementing IXamlMetadataProvider on the app
                // in that case.
                return;
            }

            IXamlType xamlType = metadataProvider.GetXamlType(pageKey);
            if (xamlType == null)
            {
                xamlType = metadataProvider.GetXamlType($"SmokSmog.Views.{pageKey}");
                if (xamlType == null) return;
            }

            var type = xamlType.UnderlyingType;
            var attr = type.GetTypeInfo().GetCustomAttribute<NavigationAttribute>();

            if (attr == null)
                return;

            var mainPage = Window.Current.Content as MainPage;
            if (mainPage == null) return;

            Frame targetFrame = null;
            switch (attr.ContentType)
            {
                case ContentType.Main:
                    targetFrame = mainPage.MainFrame;
                    break;

                case ContentType.Second:
                    targetFrame = mainPage.SecondFrame;
                    break;

                default:
                    break;
            }

            if (targetFrame != null)
            {
                if (targetFrame.Content != xamlType.UnderlyingType)
                    targetFrame.Navigate(xamlType.UnderlyingType, parameter);
            }
        }
    }
}