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

namespace PDSkeleton
{
    public static class CurrentGPS
    {
        public async static System.Threading.Tasks.Task<String> CurrentLocation()
        {
            Position position = null;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100; // 100 is highest accuracy, see Plugin doc about this value

                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        return "";
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    // get position
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);

                    string fullLocation = string.Format("Time:{0},Lat:{1},Long:{2},Altitude:{3},Altitude_Accuracy:{4},Accuracy:{5},Heading:{6},Speed:{7}",
                        position.Timestamp, position.Latitude, position.Longitude,
                        position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);
                    // accuracy - color code output
                    // store: accuracy, altitude

                    Debug.WriteLine(fullLocation);
                    
                    return position.Latitude.ToString() + "," + position.Longitude.ToString() + "," + 
                        position.Accuracy.ToString() + "," + position.Altitude.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
                return "";
            }
        }
    }
}