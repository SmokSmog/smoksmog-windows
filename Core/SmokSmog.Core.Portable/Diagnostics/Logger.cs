using System;
using System.Diagnostics;

namespace SmokSmog.Diagnostics
{
    public static class Logger
    {
        public static void Log(Exception ex)
        {
            if (Debugger.IsAttached)
                Debugger.Break();
        }
    }
}