/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:SmokSmog"
                           x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace SmokSmog.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the application and provides
    /// an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static bool _isInitialized = false;

        static ViewModelLocator()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            Initialize();
        }

        public AqiInformationViewModel AqiInformationViewModel
            => ServiceLocator.Current.GetInstance<AqiInformationViewModel>();

        public FavoritesViewModel FavoritesViewModel
            => ServiceLocator.Current.GetInstance<FavoritesViewModel>();

        public GeolocationViewModel GeolocationViewModel
            => ServiceLocator.Current.GetInstance<GeolocationViewModel>();

        public GroupedViewModel GroupedViewModel
            => ServiceLocator.Current.GetInstance<GroupedViewModel>();

        public SearchViewModel SearchViewModel
            => ServiceLocator.Current.GetInstance<SearchViewModel>();

        public SettingsViewModel SettingsViewModel
            => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public StationViewModel StationViewModel
            => ServiceLocator.Current.GetInstance<StationViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        public static void Initialize()
        {
            if (_isInitialized) return;

            SmokSmog.Services.ServiceLocator.Initialize();

            SimpleIoc.Default.Register<AqiInformationViewModel>();
            SimpleIoc.Default.Register<FavoritesViewModel>();
            SimpleIoc.Default.Register<GeolocationViewModel>();
            SimpleIoc.Default.Register<GroupedViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<StationViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();

            _isInitialized = true;
        }
    }
}