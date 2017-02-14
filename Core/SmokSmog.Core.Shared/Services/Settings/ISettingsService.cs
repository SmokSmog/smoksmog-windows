﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SmokSmog.Services.Settings
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        ObservableCollection<int> FavoritesStationsList { get; }
        int? HomeStationId { get; set; }
        Version LastLaunchedVersion { get; set; }
        string LastMainView { get; set; }
        bool LocalizationEnable { get; set; }
        bool PrimaryLiveTileEnable { get; set; }
    }
}