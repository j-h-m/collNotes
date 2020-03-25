using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.Services.Connectivity
{
    public class ConnectivityService : IConnectivityService
    {
        private const string pingUri = "https://www.google.com";
        private const int pingTimeoutms = 3000;

        public enum ActualConnectivity
        {
            Connected,
            Disconnected
        }

        public ConnectivityService() { }

        public async Task<ActualConnectivity> GetNetworkStatus()
        {
            var current = Xamarin.Essentials.Connectivity.NetworkAccess;
            var profiles = Xamarin.Essentials.Connectivity.ConnectionProfiles;

            ActualConnectivity actualConnectivity = ActualConnectivity.Disconnected;

            if (current == Xamarin.Essentials.NetworkAccess.Internet)
            {
                // Connection to internet is available
                // try pinging an address to ensure an internet connection is fully available
                // from docs: could be on WiFi but the router could have no internet
                if (profiles.Contains(Xamarin.Essentials.ConnectionProfile.WiFi) &&
                    DeviceInfo.DeviceType != DeviceType.Virtual)
                {
                    // Active Wi-Fi connection. Attempt ping before issuing connected status.
                    Ping ping = new Ping();
                    try
                    {
                        var reply = await ping.SendPingAsync(pingUri, pingTimeoutms);
                        actualConnectivity = reply.Status == IPStatus.Success ?
                            ActualConnectivity.Connected :
                            ActualConnectivity.Disconnected;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        ping.Dispose();
                    }   
                }
                else // virtual device, not WiFi
                {
                    actualConnectivity = ActualConnectivity.Connected;
                }
            }

            return actualConnectivity;
        }
    }
}
