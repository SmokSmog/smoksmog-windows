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

        private readonly NavigationStack _navigationStack = new NavigationStack(8);

        public NavigationService()
        {
#if WINDOWS_PHONE
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += (sender, args) =>
            {
                args.Handled = BackRequested();
            };
#endif

#if WINDOWS_UWP
            // Register a handler for BackRequested events and set the visibility of the Back button
            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, args) =>
            {
                args.Handled = BackRequested();
            };
#endif
        }

        public bool CanGoBack => _navigationStack.Count > 1;

        public string CurrentPageKey { get; private set; }

        public string CurrentSecondPageKey { get; private set; }

        public string LastSecondPageKey { get; private set; }

        public MainPage MainPage => Window.Current.Content as MainPage;

        /// <summary>
        /// Handles back requests and returns if request is handled
        /// </summary>
        /// <returns></returns>
        public bool BackRequested()
        {
            if (MainPage.IsMenuOpen)
            {
                MainPage.CloseMenu();
                return true;
            }

            if (MainPage.IsSearchOpen)
            {
                MainPage.CloseSearch();
                return true;
            }

            if (CanGoBack)
            {
                GoBack();
                return true;
            }

            return false;
        }

        public void GoBack()
        {
            _navigationStack.Pop();
            var item = _navigationStack.Peek();
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

            // This will happen if there are no XAML files in the project other than App.xaml. The
            // markup compiler doesn't bother implementing IXamlMetadataProvider on the app in that case.
            return null;
        }

        private void NavigateTo(string pageKey, object parameter, bool pushToStack)
        {
            if (string.IsNullOrEmpty(pageKey))
                return;

            if (MainPage.IsSearchOpen)
            {
                MainPage.CloseSearch();
            }

            var type = GetPageTypeByKey(pageKey);
            var attr = type?.GetTypeInfo().GetCustomAttribute<NavigationAttribute>();
            if (attr == null) return;

            if (MainPage == null) return;

            bool navigated = false;
            switch (attr.ContentType)
            {
                case ContentType.Main:
                    VisualStateManager.GoToState(MainPage, "MainFrameActive", true);
                    if (navigated = MainPage.MainFrame?.Navigate(type, parameter) == true)
                        CurrentPageKey = pageKey;
                    if (pushToStack && navigated)
                        _navigationStack.Push(new NavigationStack.Item(pageKey, parameter));
                    break;

                case ContentType.Second:
                    VisualStateManager.GoToState(MainPage, "SecondFrameActive", true);
                    if (navigated = MainPage.SecondFrame?.Navigate(type, parameter) == true)
                    {
                        LastSecondPageKey = CurrentSecondPageKey;
                        CurrentSecondPageKey = pageKey;
                    }
                    break;
            }

            RefreshBackButton();
        }

        private void RefreshBackButton()
        {
#if WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
#endif
        }

        private class NavigationStack
        {
            private readonly List<Item> _navigationStackList;

            internal NavigationStack(int capacity)
            {
                Capacity = capacity;
                _navigationStackList = new List<Item>(capacity);
            }

            internal int Count => _navigationStackList.Count;
            internal int Capacity { get; }

            internal Item Peek()
            {
                return _navigationStackList.LastOrDefault();
            }

            internal Item Pop()
            {
                var item = _navigationStackList.LastOrDefault();
                if (item == null) return null;

                _navigationStackList.RemoveAt(_navigationStackList.Count - 1);
                return item;
            }

            internal void Push(Item item)
            {
                var last = _navigationStackList.LastOrDefault();
                if (last != null && item.GetHashCode() == last.GetHashCode())
                    return;

                _navigationStackList.Add(item);

                if (_navigationStackList.Count > Capacity)
                    _navigationStackList.RemoveAt(0);
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