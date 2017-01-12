using SmokSmog.Resources;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection _transitions;

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            // inject WinRT Resource Manager into Resx Generated App Resources Classes
            WinRTResourceManager.InjectIntoResxGeneratedAppResources(typeof(SmokSmog.Resources.AppResources));

            SmokSmog.Services.ServiceLocator.Initialize();

            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user. Other entry points
        /// will be used when the application is launched to open a specific file, to display search
        /// results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
#if WINDOWS_PHONE || WINDOWS_UWP

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
#else

        protected override void OnLaunched(LaunchActivatedEventArgs e)
#endif
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var mainPage = Window.Current.Content as MainPage;

            // Do not repeat app initialization when the Window already has content, just ensure that
            // the window is active
            if (mainPage == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                mainPage = new MainPage();

                // TODO: change this value to a cache size that is appropriate for your application
                mainPage.ContentFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = mainPage;
            }

            if (mainPage.ContentFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (mainPage.ContentFrame.ContentTransitions != null)
                {
                    this._transitions = new TransitionCollection();
                    foreach (var c in mainPage.ContentFrame.ContentTransitions)
                    {
                        this._transitions.Add(c);
                    }
                }

                mainPage.ContentFrame.ContentTransitions = null;
                mainPage.ContentFrame.Navigated += this.RootFrame_FirstNavigated;
                mainPage.ContentFrame.NavigationFailed += OnNavigationFailed;

                // When the navigation stack isn't restored navigate to the first page, configuring
                // the new page by passing required information as a navigation parameter

                if (!mainPage.ContentFrame.Navigate(typeof(Views.StationPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
#if WINDOWS_PHONE

                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
#endif

#if WINDOWS_UWP

            // http://stackoverflow.com/questions/31594625/windows-10-mobile-cannot-hide-status-bar-statusbar-doesnt-exist-in-context
            var isStatusBarPresent = Windows
                .Foundation
                .Metadata
                .ApiInformation
                .IsTypePresent(typeof(Windows.UI.ViewManagement.StatusBar).ToString());
            if (isStatusBarPresent)
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
#endif
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">     Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended. Application state is saved without
        /// knowing whether the application will be terminated or resumed with the contents of memory
        /// still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">     Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">     Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;

            rootFrame.ContentTransitions = this._transitions ?? new TransitionCollection()
            {
#if WINDOWS_UWP || WINDOWS_PHONE
                new NavigationThemeTransition(),
#endif
            };

            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
    }
}