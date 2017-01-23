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

            foreach (var connection in connections)
            {
                if (connection == null)
                    continue;

                if (connection.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                    return true;
            }

            return false;
        }
    }
}