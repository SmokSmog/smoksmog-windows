using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace SmokSmog.Notification
{
    public sealed class TilesBackgroundTask : IBackgroundTask  //: XamlRenderingBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely while asynchronous code
            // is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                for (int i = 0; i < 5; i++)
                {
                    MemoryInfo.DebugMemoryStatus("Before Rendering Start");

                    await TileRenderer.RenderMediumTile($"LiveTile_{i}.png");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    MemoryInfo.DebugMemoryStatus("After GC Collection");
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
            finally
            {
                await MemoryInfo.SaveLog();
            }

            // Inform the system that the task is finished.
            deferral.Complete();
        }
    }
}