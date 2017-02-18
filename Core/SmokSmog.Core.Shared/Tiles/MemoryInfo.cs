using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace SmokSmog.Notification
{
    internal class MemoryInfo
    {
        public static string MemoryStatus()
        {
#if WINDOWS_UWP || WINDOWS_PHONE_APP
            var memory = MemoryManager.AppMemoryUsage;
            var memoryLimit = MemoryManager.AppMemoryUsageLimit;
            return $"Memory : \n\tused {ToMegaBytes(memory)} with limit {ToMegaBytes(memoryLimit)} MB\n\tused {ToKiloBytes(memory)} with limit {ToKiloBytes(memoryLimit)} KB";
#else
            return "Memory API is Unavailable";
#endif
        }

        private static readonly StringBuilder LogStringBuilder = new StringBuilder();

        public static void DebugMemoryStatus(string str, params object[] paraeters)
        {
            LogStringBuilder.AppendFormat(str, paraeters);
            Debug.WriteLine(str, paraeters);

            var meminfo = MemoryInfo.MemoryStatus();
            LogStringBuilder.AppendLine(meminfo);
            Debug.WriteLine(meminfo);
        }

        public static async Task SaveLog(string filename, bool append = false)
        {
            var log = LogStringBuilder.ToString();
            LogStringBuilder.Clear();

            var folder = ApplicationData.Current.LocalFolder;

            StorageFile file = null;

            if (append)
                file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            else
                file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            await FileIO.AppendTextAsync(file, log);
        }

        private static float ToMegaBytes(ulong memory)
        {
            return memory / 1024f / 1024f;
        }

        private static float ToKiloBytes(ulong memory)
        {
            return memory / 1024f;
        }
    }
}