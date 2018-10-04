using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * Help Page
 * Should eventually send the user to the help website
 * Or better yet, provide built-in instructions
 * 
 */ 

namespace PDSkeleton
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Help : ContentPage
	{
		public Help ()
		{
			InitializeComponent ();
		}

        public void btnHelp_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Will add help documentation web-page soon.");
            // Device.OpenUri(new Uri(@"https://j-h-m.github.io/pd-project-xamarin/"));
        }

    }
}