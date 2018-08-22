using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PDSkeleton
{
    // older API won't automatically ask the user for camera perms.

    public static class CurrentGPS
    {
        public async static System.Threading.Tasks.Task<Dictionary<string, double>> CurrentLocation()
        {
            Position position = null;
            Dictionary<string, double> keyValues = new Dictionary<string, double>();

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    keyValues.Add("longitude", position.Longitude);
                    keyValues.Add("latitude", position.Latitude);
                    keyValues.Add("accuracy", position.Accuracy);
                    keyValues.Add("altitude", position.Altitude);
                    keyValues.Add("altitude accuracy", position.AltitudeAccuracy);
                    keyValues.Add("heading", position.Heading);
                    return keyValues;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    // gps not available or enabled
                    Debug.WriteLine("Geolocation not available or enabled.");
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);

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

            keyValues.Add("longitude", position.Longitude);
            keyValues.Add("latitude", position.Latitude);
            keyValues.Add("accuracy", position.Accuracy);
            keyValues.Add("altitude", position.Altitude);
            keyValues.Add("altitude accuracy", position.AltitudeAccuracy);
            keyValues.Add("heading", position.Heading);

            return keyValues;
        }
    }
}