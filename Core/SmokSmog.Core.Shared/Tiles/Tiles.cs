//using Microsoft.Toolkit.Uwp.Notifications;
//using MoreLinq;
//using SmokSmog.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Windows.Foundation.Metadata;
//using Windows.System.Profile;
//using Windows.UI;
//using Windows.UI.Notifications;
//using Windows.UI.StartScreen;

namespace SmokSmog.Tiles
{
    using Device;

    public enum TileSite
    {
        Front = 0,
        Back = 1
    }

    public enum TileSize
    {
        Small = 0,
        Medium = 1,
        Wide = 2,
        Large = 3
    }

    public class Tiles
    {
        public static bool IsTileSizeSupported(TileSize tileSize)
        {
            switch (tileSize)
            {
                case TileSize.Medium:
                case TileSize.Small:
                    return true;

                case TileSize.Large:
                    var deviceType = DeviceInfo.GetDeviceType();
                    return deviceType == DeviceType.Desktop || deviceType == DeviceType.Tablet;

                case TileSize.Wide:
                    deviceType = DeviceInfo.GetDeviceType();
                    switch (deviceType)
                    {
                        case DeviceType.Phone:
                        case DeviceType.Desktop:
                        case DeviceType.Tablet:
                            return true;
                    }
                    break;
            }

            return false;
        }
    }
}

//        public static TileContent GenerateTileContent(List<Measurement> measurements)
//        {
//            return new TileContent()
//            {
//                Visual = new TileVisual()
//                {
//                    //TileSmall = GenerateTileBindingSmall(),
//                    TileMedium = GenerateTileBindingMedium(measurements),
//                    TileWide = GenerateTileBindingWide(measurements),
//                    TileLarge = GenerateTileBindingLarge(measurements),

//                    // Set the base URI for the images, so we don't redundantly specify the entire path
//                    BaseUri = new Uri("ms-appx:///Assets/Notification/")
//                }
//            };
//        }

//        public static ToastContent GenerateToastContent(List<Measurement> measurements)
//        {
//            // Start by constructing the visual portion of the toast
//            ToastBindingGeneric binding = new ToastBindingGeneric();

//            var measurement = measurements.MaxBy(o => o.Aqi.Value);

//            // We'll always have this summary text on our toast notification (it is required that
//            // your toast starts with a text element)
//            binding.Children.Add(new AdaptiveText()
//            {
//                Text = $"Stacja {measurement.Station.Name} AQI : 4.3"
//            });

//            // If Adaptive Toast Notifications are supported
//            if (IsAdaptiveToastSupported())
//            {
//                // Use the rich Tile-like visual layout
//                binding.Children.Add(GenerateMeasurementGroup(measurements, 4));
//            }

//            //// Otherwise...
//            //else
//            //{
//            //    // We'll just add two simple lines of text
//            //    binding.Children.Add(new AdaptiveText()
//            //    {
//            //        Text = "Monday ? 63? / 42?"
//            //    });

//            //    binding.Children.Add(new AdaptiveText()
//            //    {
//            //        Text = "Tuesday ? 57? / 38?"
//            //    });
//            //}

//            // Construct the entire notification
//            return new ToastContent()
//            {
//                Visual = new ToastVisual()
//                {
//                    // Use our binding from above
//                    BindingGeneric = binding,

//                    // Set the base URI for the images, so we don't redundantly specify the entire path
//                    BaseUri = new Uri("ms-appx:///Assets/Notification/")
//                },

//                // Include launch string so we know what to open when user clicks toast
//                Launch = $"view=StationPage&stationId={measurement.Station.Id}"
//            };
//        }

//        public async void PinTile(object sender, object parameters)
//        {
//            SecondaryTile tile = new SecondaryTile("SmokSmogSecondaryTile")
//            {
//                DisplayName = "SmokSmog",
//                Arguments = "args",
//            };

//            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
//            tile.VisualElements.ShowNameOnSquare310x310Logo = true;
//            tile.VisualElements.ShowNameOnWide310x150Logo = true;

//            tile.VisualElements.BackgroundColor = Color.FromArgb(255, 33, 33, 33);

//            //tile.VisualElements.Square30x30Logo =
//            //    new Uri("ms-appx:///Assets/Tiles/Medium/Square150x150Logo.png");

//#if WINDOWS_UWP || WINDOWS_PHONE
//            tile.VisualElements.Square71x71Logo =
//                new Uri("ms-appx:///Assets/Tiles/Small/Square71x71Logo.png");
//#else
//            tile.VisualElements.Square70x70Logo =
//                new Uri("ms-appx:///Assets/Tiles/Small/Square70x70Logo.png");
//#endif
//            tile.VisualElements.Square150x150Logo =
//                new Uri("ms-appx:///Assets/Tiles/Medium/Square150x150Logo.png");

//            tile.VisualElements.Wide310x150Logo =
//                new Uri("ms-appx:///Assets/Tiles/Wide/Wide310x150Logo.png");

//            tile.VisualElements.Square310x310Logo =
//                new Uri("ms-appx:///Assets/Tiles/Large/Square310x310Logo.png");

//            var result = await tile.RequestCreateAsync();
//            if (!result) return;

//            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId).EnableNotificationQueue(true);

//            var data = await LoadTestData();

//            // Generate the tile notification content and update the tile
//            TileContent content = GenerateTileContent(data);

//            // notification queue https://blogs.msdn.microsoft.com/tiles_and_toasts/2016/01/05/quickstart-how-to-use-the-tile-notification-queue-with-local-notifications/
//            var notificationXml = content.GetXml();
//            var notyfication = new TileNotification(notificationXml) { Tag = "First" };

//            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId).Update(notyfication);
//        }

//        public async void PopToast(object sender, object parameters)
//        {
//            var data = await LoadTestData();

//            // Generate the toast notification content and pop the toast
//            ToastContent content = GenerateToastContent(data);
//            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
//        }

//        public async void UpdateTile(object sender, object parameters)
//        {
//            TileUpdateManager.CreateTileUpdaterForSecondaryTile("SmokSmogSecondaryTile").EnableNotificationQueue(true);

//            var data = await LoadTestData();

//            // Generate the tile notification content and update the tile
//            TileContent content = GenerateTileContent(data);

//            // notification queue https://blogs.msdn.microsoft.com/tiles_and_toasts/2016/01/05/quickstart-how-to-use-the-tile-notification-queue-with-local-notifications/
//            var notificationXml = content.GetXml();
//            var notyfication = new TileNotification(notificationXml) { Tag = "First" };

//            TileUpdateManager.CreateTileUpdaterForSecondaryTile("SmokSmogSecondaryTile").Update(notyfication);
//        }
//        private static AdaptiveSubgroup GenerateLargeSubgroup(string header, string image, string line1, string line2)
//        {
//            // Generate the normal subgroup
//            var subgroup = GenerateSubgroup(header, image, line1, line2);

//            //// Allow there to be padding around the image
//            //if (subgroup.Children.Count > 1 && subgroup.Children[1] is AdaptiveImage)
//            //    (subgroup.Children[1] as AdaptiveImage).HintRemoveMargin = null;

//            return subgroup;
//        }

//        private static AdaptiveGroup GenerateMeasurementGroup(List<Measurement> measurements, int count = 4)
//        {
//            var group = new AdaptiveGroup();

//            for (int i = 0; i < count; i++)
//            {
//                if (measurements.Count > i)
//                {
//                    var measurement = measurements[i];
//                    string header = measurement.Parameter.ShortName;
//                    string image = measurement.Aqi.Level + ".png";
//                    string line1 = measurement.Avg1Hour?.ToString("#.");
//                    string line2 = measurement.Parameter.Unit;
//                    group.Children.Add(GenerateSubgroup(header, image, line1, line2));
//                }
//                else
//                {
//                    // if there is not enough groups generate empty
//                    group.Children.Add(new AdaptiveSubgroup()
//                    {
//                        HintWeight = 1,
//                    });
//                }
//            }
//            return group;
//        }

//        private static AdaptiveGroup GenerateStationInfoGroup(List<Measurement> measurements)
//        {
//            var measuremant = measurements.MaxBy(o => o.Aqi.Value);
//            if (measuremant == null)
//                return null;

//            return new AdaptiveGroup()
//            {
//                Children =
//                {
//                    new AdaptiveSubgroup()
//                    {
//                        HintWeight = 34,
//                        Children =
//                        {
//                            new AdaptiveImage() {Source = $"{measuremant.Aqi.Level}-square.png"},
//                        }
//                    },

//                    new AdaptiveSubgroup()
//                    {
//                        Children =
//                        {
//                            new AdaptiveText()
//                            {
//                                Text = measuremant.Aqi.Text,
//                                HintStyle = AdaptiveTextStyle.Base
//                            },

//                            new AdaptiveText()
//                            {
//                                Text = measuremant.Date.ToString("g"),
//                                HintStyle = AdaptiveTextStyle.BaseSubtle
//                            },

//                            new AdaptiveText()
//                            {
//                                Text = $"AQI : {measuremant.Aqi.Value:0.0}",
//                                HintStyle = AdaptiveTextStyle.BaseSubtle
//                            },
//                        }
//                    }
//                }
//            };
//        }

//        private static AdaptiveSubgroup GenerateSubgroup(string header, string img, string line1, string line2)
//        {
//            return new AdaptiveSubgroup()
//            {
//                HintWeight = 1,

//                Children =
//                {
//                    // Day
//                    new AdaptiveText()
//                    {
//                        Text = header,
//                        HintAlign = AdaptiveTextAlign.Center
//                    },
//                    // Image
//                    new AdaptiveImage()
//                    {
//                        Source = img,
//                        HintRemoveMargin = true
//                    },

//                    // High temp
//                    new AdaptiveText()
//                    {
//                        Text = line1,
//                        HintAlign = AdaptiveTextAlign.Center
//                    },

//                    // Low temp
//                    new AdaptiveText()
//                    {
//                        Text = line2,
//                        HintAlign = AdaptiveTextAlign.Center,
//                        HintStyle = AdaptiveTextStyle.CaptionSubtle
//                    }
//                }
//            };
//        }

//        //private static AdaptiveText GenerateLegacyToastText(string day, string weatherEmoji, int tempHi, int tempLo)
//        //{
//        //    return new AdaptiveText()
//        //    {
//        //        Text = $"{day} {weatherEmoji} {tempHi}? / {tempLo}?"
//        //    };
//        //}
//        private static TileBinding GenerateTileBindingLarge(List<Measurement> measurements)
//        {
//            var measuremant = measurements.MaxBy(o => o.Aqi.Value);
//            if (measuremant == null)
//                return null;

//            return new TileBinding()
//            {
//                Content = new TileBindingContentAdaptive()
//                {
//                    Children =
//                    {
//                        GenerateStationInfoGroup(measurements),

//                        //For spacing
//                        new AdaptiveText(),

//                        GenerateMeasurementGroup(measurements, 4)
//                    }
//                }
//            };
//        }

//        private static TileBinding GenerateTileBindingMedium(List<Measurement> measurements)
//        {
//            return new TileBinding()
//            {
//                Content = new TileBindingContentAdaptive()
//                {
//                    Children =
//                    {
//                        GenerateMeasurementGroup(measurements, 2)
//                    }
//                }
//            };
//        }

//        private static TileBinding GenerateTileBindingWide(List<Measurement> measurements)
//        {
//            return new TileBinding()
//            {
//                Content = new TileBindingContentAdaptive()
//                {
//                    Children =
//                    {
//                        GenerateMeasurementGroup(measurements, 4)
//                    }
//                }
//            };
//        }

//        //TODO - figure it out how to the smallest tile should look like - not used now
//        //private static TileBinding GenerateTileBindingSmall(Station station, List<ParameterWithMeasurements> parameterwith)
//        //{
//        //    return new TileBinding()
//        //    {
//        //        Content = new TileBindingContentAdaptive()
//        //        {
//        //            TextStacking = TileTextStacking.Center,
//        //            Children =
//        //            {
//        //                new AdaptiveText()
//        //                {
//        //                    Text = "Mon",
//        //                    HintStyle = AdaptiveTextStyle.Body,
//        //                    HintAlign = AdaptiveTextAlign.Center
//        //                },
//        //                new AdaptiveText()
//        //                {
//        //                    Text = "63?",
//        //                    HintStyle = AdaptiveTextStyle.Base,
//        //                    HintAlign = AdaptiveTextAlign.Center
//        //                }
//        //            }
//        //        }
//        //    };
//        //}
//        private static bool IsAdaptiveToastSupported()
//        {
//#if WINDOWS_UWP
//            switch (AnalyticsInfo.VersionInfo.DeviceFamily)
//            {
//                // Desktop and Mobile started supporting adaptive toasts in API contract 3
//                // (Anniversary Update)
//                case "Windows.Mobile":
//                case "Windows.Desktop":
//                    return ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3);

//                // Other device families do not support adaptive toasts
//                default:
//                    return false;
//            }
//#else
//            return false;
//#endif
//        }

//        private async Task<List<Measurement>> LoadTestData()
//        {
//            try
//            {
//                var dataService = new Services.ServiceLocator().DataService;
//                var station = await dataService.GetStationAsync(4);
//                var parameters = await dataService.GetParametersAsync(station);
//                var measurements = await dataService.GetMeasurementsAsync(station, parameters);
//                return measurements.OrderByDescending(o => o.Aqi.Value).ToList();
//            }
//            catch (Exception ex)
//            {
//                Diagnostics.Logger.Log(ex);
//                //throw;
//            }

//            return null;
//        }
//    }
//}