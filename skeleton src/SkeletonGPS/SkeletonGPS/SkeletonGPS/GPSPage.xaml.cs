using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Diagnostics;

namespace SkeletonGPS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GPSPage : ContentPage
    {
        public static string GPSLocation { get; set; }

		public GPSPage ()
		{
			InitializeComponent ();
		}

        public async void OnClick_GPS(object sender, EventArgs e)
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    lblGPS.Text = position.ToString();
                    GPSLocation = position.ToString();
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    lblGPS.Text = "Geolocation not available or enabled.";
                    Debug.WriteLine("Geolocation not available or enabled.");
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
            {
                lblGPS.Text = "Geolocation returned null.";
                Debug.WriteLine("Geolocation returned null.");
            }

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);
            lblGPS.Text = output.ToString();
            GPSLocation = output.ToString();
        }
	}
}