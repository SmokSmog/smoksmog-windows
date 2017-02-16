#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SmokSmog.Services.Notification
{
    public interface ITilesService : INotifyPropertyChanged
    {
        bool CanRegisterBackgroundTasks { get; }
        bool IsPrimaryTileNotificationEnable { get; set; }
        bool IsPrimaryTileTimerUpdateRegidtered { get; }
        DateTime? PrimaryTileLastUpdate { get; set; }

        Task Initialize();

        Task UpdatePrimaryTile();
    }
}

#endif