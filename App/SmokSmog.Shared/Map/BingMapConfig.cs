namespace SmokSmog.Map
{
    public class BingMapConfig
    {
#if WINDOWS_UWP
        public string Token => "HERE_PLACE_YOUR_BING_MAPS_TOKEN";
#elif WINDOWS_APP
        public string Token => "HERE_PLACE_YOUR_BING_MAPS_TOKEN";
#elif WINDOWS_PHONE
        public string Token => "HERE_PLACE_YOUR_BING_MAPS_TOKEN";
#endif
    }
}