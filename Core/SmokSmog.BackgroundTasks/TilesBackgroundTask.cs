﻿using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace SmokSmog.BackgroundTasks
{
    public sealed class TilesBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely while asynchronous code
            // is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            UpdateTile();

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        private static void UpdateTile()
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            // Keep track of the number feed items that get tile notifications.
            //int itemCount = 0;

            //// Create a tile notification for each feed item.
            //foreach (var item in feed.Items)
            //{
            //    XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText03);

            // var title = item.Title; string titleText = title.Text == null ? String.Empty :
            // title.Text; tileXml.GetElementsByTagName(textElementName)[0].InnerText = titleText;

            // // Create a new tile notification. updater.Update(new TileNotification(tileXml));

            //    // Don't create more than 5 notifications.
            //    if (itemCount++ > 5) break;
            //}
        }
    }
}