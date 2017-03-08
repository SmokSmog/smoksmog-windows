using Windows.UI.Xaml.Controls;

namespace SmokSmog.Views
{
    using Navigation;

    [Navigation(ContentType = ContentType.Main)]
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        //private async void GetBackgroundLog()
        //{
        //    var exelogFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("BackgroundTask.Execution.log", CreationCollisionOption.OpenIfExists);
        //    var memlogFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("BackgroundTask.Memory.log", CreationCollisionOption.OpenIfExists);

        //    StringBuilder logBuilder = new StringBuilder();

        //    if (exelogFile != null)
        //    {
        //        logBuilder.AppendLine("BackgroundTask.Execution.log");
        //        var log = await FileIO.ReadTextAsync(exelogFile);
        //        logBuilder.AppendLine(log);
        //    }
        //    else
        //    {
        //        logBuilder.Append("No found Execution log file");
        //    }

        //    logBuilder.AppendLine("----------------------");

        //    if (memlogFile != null)
        //    {
        //        logBuilder.AppendLine("BackgroundTask.Memory.log");
        //        var log = await FileIO.ReadTextAsync(memlogFile);
        //        logBuilder.AppendLine(log);
        //    }
        //    else
        //    {
        //        logBuilder.Append("No found Memory log file");
        //    }

        //    LogTextBlock.Text = logBuilder.ToString();
        //}

        //private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        //{
        //    if (ShowLogSwitch.IsOn)
        //        GetBackgroundLog();
        //    else
        //        LogTextBlock.Text = string.Empty;
        //}
    }
}