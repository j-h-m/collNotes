using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;

/*
 * Uses Plugin.Geolocator package to get the user's current GPS location.
 * Returns a string containing the latitude, longitude coordinate pair as well as error and altitude in Meters.
 */

namespace PDSkeleton
{
    // consider permissions on all devices

    public static class CurrentGPS
    {
        public async static System.Threading.Tasks.Task<String> CurrentLocation()
        {
            Position position = null;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100; // highest accuracy, see Plugin doc about this value

                //position = await locator.GetLastKnownLocationAsync();

                //if (position != null)
                //{
                //    return position.Latitude.ToString() + ", " + position.Longitude.ToString() + "," + position.Accuracy.ToString() + "," + position.Altitude.ToString();
                //}

                //if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                //{
                //    // gps not available or enabled
                //    Debug.WriteLine("Geolocation not available or enabled.");
                //}

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, true); // probably should get new location each time

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
            {
                Debug.WriteLine("GPS returned null.");
            }

            Debug.WriteLine(string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed));

            return position.Latitude.ToString() + "," + position.Longitude.ToString() + "," + position.Accuracy.ToString() + "," + position.Altitude.ToString();
        }
    }
}