using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace SkeletonGPS
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        public async void OnClick_GoToCSV(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CSVExport());
        }

        public async void OnClick_GoToGPS(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GPSPage());
        }

    }
}
