using System.Linq;
using Windows.Networking.Connectivity;

namespace SmokSmog.Services.Network
{
    public class NetworkInformation
    {
        public static bool HasInternetConnection()
        {
            var connections = Windows.Networking.Connectivity.NetworkInformation.GetConnectionProfiles().ToList();
            connections.Add(Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile());
            return connections.Any(connection => connection?.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }
    }
}