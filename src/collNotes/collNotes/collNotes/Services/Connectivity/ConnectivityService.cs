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
        public enum ActualConnectivity
        {
            Connected,
            Disconnected
        }

        public ConnectivityService() { }

        public ActualConnectivity GetNetworkStatus()
        {
            var current = Xamarin.Essentials.Connectivity.NetworkAccess;

            ActualConnectivity actualConnectivity = ActualConnectivity.Disconnected;

            if (current == Xamarin.Essentials.NetworkAccess.Internet)
            {
                actualConnectivity = ActualConnectivity.Connected;
            }

            return actualConnectivity;
        }
    }
}
