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

        public static void Initialize()
        {
            if (!_isInitialized)
            {
                SimpleIoc.Default.Register<StationListViewModel>();
                SimpleIoc.Default.Register<FavoritesViewModel>();
                _isInitialized = true;
            }
        }

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

        public StationListViewModel StationList
        {
            get { return ServiceLocator.Current.GetInstance<StationListViewModel>(); }
        }

        public FavoritesViewModel FavoritesViewModel
        {
            get { return ServiceLocator.Current.GetInstance<FavoritesViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}