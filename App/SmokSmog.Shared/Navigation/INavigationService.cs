namespace SmokSmog.Navigation
{
    public interface INavigationService : GalaSoft.MvvmLight.Views.INavigationService
    {
        string CurrentSecondPageKey { get; }
    }
}