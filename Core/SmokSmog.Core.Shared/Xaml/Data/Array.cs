/*
Conditional Compilation Symbols

Windows Universal 10  - WINDOWS_UWP
Windows Store 8.1     - WINDOWS_APP
Windows Phone 8.1     - WINDOWS_PHONE_APP
Windows Phone 8.0     - WINDOWS_PHONE
WPF .net45            - WINDOWS_DESKTOP

*/

#if !PORTABLE

using System.Collections.ObjectModel;

namespace SmokSmog.Xaml.Data
{
    public class Array : ObservableCollection<object> { }
}

#endif