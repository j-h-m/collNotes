using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Diagnostics;

/*
 * Uses Plugin.Geolocator package to get the user's current GPS location.
 * Returns a string containing the latitude, longitude coordinate pair as well as error and altitude in Meters.
 */

namespace collnotes.Plugins
{
    public static class CurrentGPS
    {
        public async static System.Threading.Tasks.Task<Position> CurrentLocation()
        {
            Position position = null;

            try
            {
                var locator = CrossGeolocator.Current;
                // locator.DesiredAccuracy = 100; // 100 is highest accuracy, see Plugin doc about this value
                locator.DesiredAccuracy = 50;

                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        return null;
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                    {
                        status = results[Permission.Location];
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    // position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);
                    return position;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
                return null;
            }
        }
    }
}