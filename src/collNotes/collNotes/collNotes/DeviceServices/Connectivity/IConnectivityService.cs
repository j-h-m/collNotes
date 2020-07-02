using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static collNotes.DeviceServices.Connectivity.ConnectivityService;

namespace collNotes.DeviceServices.Connectivity
{
    public interface IConnectivityService
    {
        ActualConnectivity GetNetworkStatus();
    }
}
