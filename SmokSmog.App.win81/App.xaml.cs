﻿using SmokSmog.Globalization;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace SmokSmog
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private TransitionCollection _transitions;

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            // inject WinRT Resource Manager into Resx Generated App Resources Classes
            WinRTResourceManager.InjectIntoResxGeneratedAppResources(typeof(SmokSmog.Resources.AppResources));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user. Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
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

                // When the navigation stack isn't restored navigate to the first page, configuring
                // the new page by passing required information as a navigation parameter

                if (!mainPage.ContentFrame.Navigate(typeof(Views.SationList), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();

            //await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            //rootFrame.ContentTransitions = this._transitions ?? new TransitionCollection() { new Windows.UI.Xaml.Media.Animation NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended. Application state is saved without
        /// knowing whether the application will be terminated or resumed with the contents of memory
        /// still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}