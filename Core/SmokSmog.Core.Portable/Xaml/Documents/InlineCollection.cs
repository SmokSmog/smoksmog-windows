/*
Conditional Compilation Symbols

Windows Universal 10  - WINDOWS_UWP
Windows Store 8.1     - WINDOWS_APP
Windows Phone 8.1     - WINDOWS_PHONE_APP
Windows Phone 8.0     - WINDOWS_PHONE
WPF .net45            - WINDOWS_DESKTOP

*/

#if !PORTABLE

using System.Collections.Generic;

#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINRT

using Windows.UI.Xaml.Documents;

#endif

#if WINDOWS_DESKTOP || WINDOWS_PHONE || DESKTOP

using System.Windows.Documents;

#endif

namespace SmokSmog.Xaml.Documents
{
    public class InlineCollection : List<Inline>, IEnumerable<Inline> { }
}

#endif