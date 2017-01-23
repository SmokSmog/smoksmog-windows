namespace SmokSmog.Navigation
{
    public interface INavigationProvider
    {
        SmokSmog.Navigation.INavigationService NavigationService { get; }
    }
}