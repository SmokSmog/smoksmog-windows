using SmokSmog.Diagnostics;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;

namespace SmokSmog.Notification
{
    public sealed class TilesBackgroundTask : IBackgroundTask  //: XamlRenderingBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely while asynchronous code
            // is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            await RenderTiles();

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        public IAsyncAction RenderTilesAction()
        {
            return RenderTiles().AsAsyncAction();
        }

        internal async Task RenderTiles()
        {
            TileRenderer tileRenderer = new TileRenderer();

            try
            {
                for (int i = 0; i < 1; i++)
                {
                    MemoryInfo.DebugMemoryStatus("Before Rendering Start");

                    await tileRenderer.RenderMediumTileBack($"LiveTileBack_{i}.png");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    await tileRenderer.RenderMediumTileFront($"LiveTileFront_{i}.png");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    MemoryInfo.DebugMemoryStatus("After GC Collection");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
            finally
            {
                await MemoryInfo.SaveLog();
            }
        }
    }
}