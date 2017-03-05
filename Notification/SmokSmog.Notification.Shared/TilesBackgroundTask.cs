using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace SmokSmog.Notification
{
    using Tiles;

    public sealed class TilesBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskCancellationReason _cancelReason = BackgroundTaskCancellationReason.Abort;
        private BackgroundTaskDeferral _deferral;
        //private volatile bool _cancelRequested = false;

        /// <summary>
        /// Primary Tile Updater Background Task
        /// </summary>
        /// <seealso cref="https://code.msdn.microsoft.com/windowsapps/Background-Task-Sample-9209ade9"/>
        /// <param name="taskInstance"></param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                // Get a deferral, to prevent the task from closing prematurely while asynchronous code
                // is still running.
                _deferral = taskInstance.GetDeferral();

                // Query BackgroundWorkCost
                // Guidance: If BackgroundWorkCost is high, then perform only the minimum amount
                // of work in the background task and return immediately.
                var log = await ApplicationData.Current.LocalFolder.CreateFileAsync("BackgroundTask.Execution.log", CreationCollisionOption.ReplaceExisting);
                await FileIO.AppendTextAsync(log, $"\"Start\": \"{DateTime.Now:O}\", " +
                                                  $"\"Cost\": \"{BackgroundWorkCost.CurrentBackgroundWorkCost}\",");

                // Associate a cancellation handler with the background task.
                taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

                TilesUpdater tilesUpdater = new TilesUpdater();
                await tilesUpdater.PrimaryTileRenderAndUpdate();

                log = await ApplicationData.Current.LocalFolder.CreateFileAsync("BackgroundTask.Execution.log", CreationCollisionOption.OpenIfExists);
                await FileIO.AppendTextAsync(log, $"\"End\" : \"{DateTime.Now:O}\"");
            }
            finally
            {
                // Inform the system that the task is finished.
                _deferral?.Complete();
            }
        }

        // Handles background task cancellation.
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //
            // Indicate that the background task is canceled.
            //
            //_cancelRequested = true;
            _cancelReason = reason;

            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }
    }
}