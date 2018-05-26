using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace HelloGPS
{
	public partial class MainPage : ContentPage
	{
        private string latitude = "";
        private string longitude = "";

		public MainPage()
		{
			InitializeComponent();
		}

        public void OnCSVClick()
        {

        }

        public async void OnGPSClick()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    var results = await CrossGeolocator.Current.GetPositionAsync();
                    longitude = results.Longitude.ToString();
                    latitude = results.Latitude.ToString();
                    lblGPS.Text = "Current Location:\nLat: " + latitude + "\nLong: " + longitude;
                }
                else
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                lblGPS.Text = "Error: " + ex;
            }
        }
    }
}
