using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PDSkeleton
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        public async void Trip_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Trip());
        }

        public async void Settings_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Settings());
        }

        public async void Help_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Help());
        }
    }
}
