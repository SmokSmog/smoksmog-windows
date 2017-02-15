#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP

using System;
using System.Threading.Tasks;

namespace SmokSmog.Services.Notification
{
    public interface ITilesService
    {
        bool CanRegisterBackgroundTasks { get; }
        bool IsPrimaryTileNotificationEnable { get; }
        bool IsPrimaryTileTimerUpdateRegidtered { get; }
        bool IsSecondaryTilesNotificationEnable { get; }
        DateTime? PrimaryTileLastUpdate { get; set; }

        Task Initialize();

        Task<bool> RegisterBackgroundTasks();

        void UnregisterTasks();
    }
}

#endif