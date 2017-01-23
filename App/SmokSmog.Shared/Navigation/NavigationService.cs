using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace SmokSmog.Navigation
{
    public class NavigationService : INavigationService
    {
        public const string RootPageKey = "-- ROOT --";
        public const string UnknownPageKey = "-- UNKNOWN --";

        private readonly NavigationStack _navigationStack = new NavigationStack();

        public NavigationService()
        {
#if WINDOWS_PHONE
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += (sender, args) =>
            {
                if (!CanGoBack) return;
                args.Handled = true;
                GoBack();
            };
#endif

#if WINDOWS_UWP
            // Register a handler for BackRequested events and set the
            // visibility of the Back button
            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, args) =>
            {
                if (!CanGoBack) return;
                args.Handled = true;
                GoBack();
            };

            _navigationStack.ItemsChanged += _navigationStack_ItemsChanged;
#endif
        }

#if WINDOWS_UWP
        private void _navigationStack_ItemsChanged()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }
#endif

        public bool CanGoBack => _navigationStack.Count > 1;
        public string CurrentPageKey { get; private set; }
        public string CurrentSecondPageKey { get; private set; }

        public void GoBack()
        {
            var item = _navigationStack.Pop();
            if (item != null)
                NavigateTo(item.PageKey, item.Parameter, false);
        }

        public void NavigateTo(string pageKey)
            => NavigateTo(pageKey, null);

        public void NavigateTo(string pageKey, object parameter)
            => NavigateTo(pageKey, parameter, true);

        private Type GetPageTypeByKey(string pageKey)
        {
            if (string.IsNullOrEmpty(pageKey))
                return null;

            IXamlMetadataProvider metadataProvider = Application.Current as IXamlMetadataProvider;

            IXamlType xamlType = metadataProvider?.GetXamlType(pageKey);
            if (xamlType != null)
                return xamlType.UnderlyingType;

            xamlType = metadataProvider?.GetXamlType($"SmokSmog.Views.{pageKey}");
            if (xamlType != null) return xamlType.UnderlyingType;

            // This will happen if there are no XAML files in the project other than App.xaml.
            // The markup compiler doesn't bother implementing IXamlMetadataProvider on the app
            // in that case.
            return null;
        }

        private void NavigateTo(string pageKey, object parameter, bool pushToStack)
        {
            if (string.IsNullOrEmpty(pageKey))
                return;

            var type = GetPageTypeByKey(pageKey);
            var attr = type?.GetTypeInfo().GetCustomAttribute<NavigationAttribute>();
            if (attr == null) return;

            var mainPage = Window.Current.Content as MainPage;
            if (mainPage == null) return;

            switch (attr.ContentType)
            {
                case ContentType.Main:
                    VisualStateManager.GoToState(mainPage, "MainFrameActive", true);
                    if (mainPage.MainFrame?.Navigate(type, parameter) == true)
                    {
                        if (pushToStack)
                            _navigationStack.Push(new NavigationStack.Item(pageKey, parameter));
                        CurrentPageKey = pageKey;
                    }
                    break;

                case ContentType.Second:
                    VisualStateManager.GoToState(mainPage, "SecondFrameActive", true);
                    if (mainPage.SecondFrame?.Navigate(type, parameter) == true)
                        CurrentSecondPageKey = pageKey;
                    break;
            }
        }

        private class NavigationStack
        {
            private readonly List<Item> _navigationStack = new List<Item>();
            public int Count => _navigationStack.Count;

            internal Item Pop()
            {
                var item = _navigationStack.LastOrDefault();
                if (item == null) return null;

                _navigationStack.RemoveAt(_navigationStack.Count - 1);
                ItemsChanged?.Invoke();
                return item;
            }

            internal event Action ItemsChanged;

            internal void Push(Item item)
            {
                var last = _navigationStack.LastOrDefault();
                if (last != null && item.GetHashCode() == last.GetHashCode())
                    return;

                _navigationStack.Add(item);
                ItemsChanged?.Invoke();

                if (_navigationStack.Count > 5)
                    _navigationStack.RemoveAt(0);
            }

            internal class Item
            {
                public Item(string pageKey, object parameter)
                {
                    PageKey = pageKey;
                    Parameter = parameter;
                }

                public string PageKey { get; }

                public object Parameter { get; }

                public override int GetHashCode()
                {
                    int hash = 41;
                    if (PageKey != null)
                        hash = unchecked(hash * 59 + PageKey.GetHashCode());

                    if (Parameter != null)
                        hash = unchecked(hash * 59 + Parameter.GetHashCode());

                    return hash;
                }
            }
        }
    }
}