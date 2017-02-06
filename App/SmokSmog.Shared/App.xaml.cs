using SmokSmog.Diagnostics;
using SmokSmog.Globalization;
using SmokSmog.Navigation;
using SmokSmog.Resources;
using SmokSmog.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application, INavigationProvider
    {
        private void RegisterBackgroundTasks()
        {
            var builder = new BackgroundTaskBuilder()
            {
                Name = "SmokSmog.Core.Tiles",
                TaskEntryPoint = "SmokSmog.Core.Tiles.TilesBackgroundTask"
            };

            IBackgroundTrigger trigger = new TimeTrigger(15, false);
            builder.SetTrigger(trigger);

            IBackgroundCondition condition = new SystemCondition(SystemConditionType.InternetAvailable);
            builder.AddCondition(condition);

            IBackgroundTaskRegistration task = builder.Register();

            //YOu have the option of implementing these events to do something upon completion
            //task.Progress += task_Progress;
            //task.Completed += task_Completed;
        }

        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            // inject WinRT Resource Manager into Resx Generated App Resources Classes
            WinRTResourceManager.InjectIntoResxGeneratedAppResources(typeof(SmokSmog.Resources.AppResources));

            Services.ServiceLocator.Initialize();

            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        public SmokSmog.Navigation.INavigationService NavigationService
        {
            get
            {
                object obj = null;
                Resources.TryGetValue("NavigationService", out obj);
                return obj as SmokSmog.Navigation.NavigationService;
            }
        }

        public SmokSmog.Services.IServiceLocator ServiceLocator
        {
            get
            {
                object obj = null;
                Resources.TryGetValue("ServiceLocator", out obj);
                return obj as SmokSmog.Services.IServiceLocator;
            }
        }

        public ViewModelLocator ViewModelLocator
        {
            get
            {
                object obj = null;
                Resources.TryGetValue("ViewModelLocator", out obj);
                return obj as ViewModelLocator;
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user. Other entry points
        /// will be used when the application is launched to open a specific file, to display search
        /// results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            RegisterBackgroundTasks();

            await CheckInternetConnection();

            var mainPage = Window.Current.Content as MainPage;

            // Do not repeat app initialization when the Window already has content, just ensure that
            // the window is active
            if (mainPage == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                mainPage = new MainPage();

                // TODO: change this value to a cache size that is appropriate for your application
                //mainPage.ContentFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = mainPage;
            }

            //if (mainPage.ContentFrame.Content == null)
            //{
            //    // Removes the turnstile navigation for startup.
            //    if (mainPage.ContentFrame.ContentTransitions != null)
            //    {
            //        this._transitions = new TransitionCollection();
            //        foreach (var c in mainPage.ContentFrame.ContentTransitions)
            //        {
            //            this._transitions.Add(c);
            //        }
            //    }

            // mainPage.ContentFrame.ContentTransitions = null; mainPage.ContentFrame.Navigated +=
            // this.RootFrame_FirstNavigated; mainPage.ContentFrame.NavigationFailed += OnNavigationFailed;

            // // When the navigation stack isn't restored navigate to the first page, configuring //
            // the new page by passing required information as a navigation parameter

            //    if (!mainPage.ContentFrame.Navigate(typeof(Views.StationPage), e.Arguments))
            //    {
            //        throw new Exception("Failed to create initial page");
            //    }
            //}

            // Ensure the current window is active
            Window.Current.Activate();

#if WINDOWS_UWP || WINDOWS_PHONE

            // http://stackoverflow.com/questions/31594625/windows-10-mobile-cannot-hide-status-bar-statusbar-doesnt-exist-in-context
            try
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                if (statusBar != null)
                    await statusBar.HideAsync();
            }
            catch (Exception exception)
            {
                Logger.Log(exception);
            }
#endif
        }

        private async Task CheckInternetConnection()
        {
            try
            {
                var locator = Resources["ServiceLocator"] as SmokSmog.Services.IServiceLocator;
                var stations = await locator.DataService.GetStationsAsync();
            }
            catch (Exception ex)
            {
                SmokSmog.Diagnostics.Logger.Log(ex);

                // Create the message dialog and set its content
                var messageDialog = new MessageDialog(LocalizedStrings.LocalizedString("StringNoApiConnection"));

                // Add commands and set their callbacks; both buttons use the same callback function
                // instead of inline event handlers
                messageDialog.Commands.Add(new UICommand(
                    LocalizedStrings.LocalizedString("StringClose"),
                    new UICommandInvokedHandler(command => { Exit(); })));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                messageDialog.CancelCommandIndex = 1;

                // Show the message dialog
                await messageDialog.ShowAsync();
            }
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
    }
}