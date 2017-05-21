using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmokSmog.Diagnostics;
using SmokSmog.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace SmokSmog.ViewModel
{
    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

    public interface IState : INotifyPropertyChanged
    {
        UIElement Content { get; }
        string Message { get; }
        string Name { get; }
        bool SupportContent { get; }
    }

    public interface IStatefulViewModel : INotifyPropertyChanged
    {
        event StateChangedEventHandler StateChanged;

        RelayCommand CancelCommand { get; }
        RelayCommand ReloadCommand { get; }

        IState State { get; }

        Task Load(object parameter);

        Task Reload();

        Task Unload();
    }

    public class BusyState : StateBase
    {
        private bool _indeterminate = true;

        private double _progress = 0.0;

        public BusyState() : base("Loading")
        {
            Message = NameLocalized + "...";
        }

        public bool Indeterminate
        {
            get { return _indeterminate; }
            set
            {
                if (_indeterminate == value) return;
                _indeterminate = value;
                RaisePropertyChanged();
            }
        }

        public double Progress
        {
            get { return _progress; }
            set
            {
                if (Math.Abs(_progress - value) < 0.0001) return;
                _progress = value;
                RaisePropertyChanged();
            }
        }

        public override bool SupportContent => true;

        protected override UIElement ContentFactory()
        {
            var c = new StackPanel() { Background = new SolidColorBrush(Colors.White), DataContext = this };
            c.Children.Add(new ProgressRing() { Width = 100, Height = 100, IsActive = true });

            var tb = new TextBlock() { Width = 200, FontSize = 15 };
            tb.SetBinding(TextBlock.TextProperty, new Binding() { Path = new PropertyPath(nameof(Message)) });
            c.Children.Add(tb);
            return c;
        }
    }

    public class CanceledState : StateBase
    {
        public CanceledState() : base("CanceledState")
        {
        }
    }

    public class ErrorState : StateBase
    {
        public ErrorState() : base("Error")
        {
        }

        public ErrorState(Exception exception) : this()
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }

    public class NotInitializedState : StateBase
    {
        public NotInitializedState()
            : base("NotInitialized")
        {
        }
    }

    public class ReadyState : StateBase
    {
        public ReadyState() : base("Ready")
        {
        }
    }

    public abstract class StateBase : ObservableObject, IState
    {
        private string _message = string.Empty;

        protected StateBase(string name)
        {
            Name = name;
        }

        public UIElement Content => SupportContent ? ContentFactory() : null;

        public string Message
        {
            get { return _message; }
            protected set
            {
                if (_message == value) return;
                _message = value;
                RaisePropertyChanged();
            }
        }

        public string Name { get; }

        public string NameLocalized => LocalizedStrings.GetString(Name);
        public virtual bool SupportContent { get; } = false;

        protected virtual UIElement ContentFactory()
        {
            return null;
        }
    }

    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(IState oldState, IState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public IState NewState { get; }
        public IState OldState { get; }
    }

    public abstract class StatefulViewModelBase : ViewModelBase, IStatefulViewModel
    {
        private RelayCommand _cancelCommand;
        private CancellationTokenSource _cts = null;
        private IState _previousState;
        private RelayCommand _reloadCommand;
        private IState _state = new NotInitializedState();

        public event StateChangedEventHandler StateChanged;

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(
                           CancelOperation,
                           () => _cts != null));
            }
        }

        public RelayCommand ReloadCommand
        {
            get
            {
                return _reloadCommand ?? (_reloadCommand = new RelayCommand(
                    async () => await Reload(),
                    () => State is ReadyState));
            }
        }

        public IState State
        {
            get { return _state; }
            private set
            {
                if (_state == value) return;
                var old = _state;
                _state = value;
                RaisePropertyChanged();
                StateChanged?.Invoke(this, new StateChangedEventArgs(old, _state));
            }
        }

        public async Task Load(object parameter) => await StartOperation(OnLoad, parameter);

        public async Task Reload() => await StartOperation(OnReload);

        public async Task Unload() => await StartOperation(OnUnload);

        protected abstract Task<IState> OnLoad(object parameter, CancellationToken token);

        protected abstract Task<IState> OnReload(CancellationToken token);

        protected virtual Task<IState> OnUnload(CancellationToken token)
        {
            IState state = new NotInitializedState();
            return Task.FromResult(state);
        }

        private void CancelOperation()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;

                State = _previousState;
            }
        }

        private Task StartOperation(Func<CancellationToken, Task<IState>> task)
        {
            Func<object, CancellationToken, Task<IState>> innerTask = async (o, token) => await task.Invoke(token);
            return StartOperation(innerTask, null);
        }

        private async Task StartOperation(Func<object, CancellationToken, Task<IState>> task, object parameter)
        {
            try
            {
                CancelOperation();
                _cts = new CancellationTokenSource();
                _previousState = State;

                State = new BusyState();
                State = await task.Invoke(parameter, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // by default previous state will be set
                if (State is NotInitializedState)
                    State = new CanceledState();
            }
            catch (Exception exception)
            {
                State = new ErrorState(exception);
                Logger.Log(exception);
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }
    }
}