using GalaSoft.MvvmLight;
using SmokSmog.Resources;
using System;
using System.ComponentModel;

namespace SmokSmog.ViewModel
{
    public interface IViewModelState
    {
        string Name { get; }
    }

    public class ReadyViewModelState : IViewModelState
    {
        public string Name => LocalizedStrings.GetString("Ready");
    }

    public class ErrorViewModelState : IViewModelState
    {
        public string Name => LocalizedStrings.GetString("Error");

        public string ErrorInfo { get; }

        public Exception Exception { get; }

        public ErrorViewModelState()
        {
        }

        public ErrorViewModelState(Exception exception)
        {
            Exception = exception;
        }
    }

    public class NotInitializedViewModelState : IViewModelState
    {
        public string Name => LocalizedStrings.GetString("NotInitialized");
    }

    public class InitializingViewModelState : IViewModelState
    {
        public string Name => LocalizedStrings.GetString("Initializing");
    }

    public interface IStatefulViewModel : INotifyPropertyChanged
    {
        IViewModelState State { get; }
    }

    public class StatefulViewModelBase : ViewModelBase, IStatefulViewModel
    {
        public IViewModelState State { get; }
    }
}