using System.Threading.Tasks;
using SmokSmog.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SmokSmog.Xaml.UI
{
    public interface IAsyncPage
    {
        Task OnNavigatedFrom(NavigationEventArgs e);

        Task OnNavigatedTo(NavigationEventArgs e);
    }

    public abstract class PageAsync : Page, IAsyncPage
    {
        async Task IAsyncPage.OnNavigatedFrom(NavigationEventArgs e)
        {
            await OnUnload();
        }

        async Task IAsyncPage.OnNavigatedTo(NavigationEventArgs e)
        {
            await OnLoad(e.Parameter);
        }

        public async Task Reload(object parameter)
        {
            await OnReload();
        }

        protected abstract Task OnLoad(object parameter);

        protected sealed override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            await (this as IAsyncPage).OnNavigatedFrom(e);
            base.OnNavigatedFrom(e);
        }

        protected sealed override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await (this as IAsyncPage).OnNavigatedTo(e);
        }

        protected abstract Task OnReload();

        protected abstract Task OnUnload();
    }

    public class StatefulPage : PageAsync
    {
        public IStatefulViewModel StatefulViewModel => DataContext as IStatefulViewModel;

        private UIElement _content;

        protected override async Task OnLoad(object parameter)
        {
            // store destination content
            _content = Content;

            if (StatefulViewModel != null)
            {
                StatefulViewModel.StateChanged += StatefulViewModel_StateChanged;

                await StatefulViewModel.Load(parameter);
            }
        }

        private void StatefulViewModel_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e?.NewState?.SupportContent == true)
            {
                Content = e.NewState.Content;
                return;
            }

            Content = _content;
        }

        protected override async Task OnReload()
        {
            if (StatefulViewModel != null)
                await StatefulViewModel.Reload();
        }

        protected override async Task OnUnload()
        {
            if (StatefulViewModel != null)
            {
                await StatefulViewModel.Unload();
                StatefulViewModel.StateChanged -= StatefulViewModel_StateChanged;
            }
        }
    }
}