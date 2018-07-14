using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDSkeleton
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TripPage : ContentPage
	{
        private string primaryCollector = "";
        private string additionalCollectors = "";
        private string projectName = "";
        private DateTime collectionDate;
        private Trip trip;

		public TripPage()
		{
            trip = new Trip();
			InitializeComponent ();
		}

        public void PrimaryCollector_EntryCompleted(object sender, EventArgs e)
        {
            primaryCollector = ((Entry)sender).Text;
        }

        public void AdditionalCollectors_EntryCompleted(object sender, EventArgs e)
        {
            additionalCollectors = ((Entry)sender).Text;
        }

        public void ProjectName_EntryCompleted(object sender, EventArgs e)
        {
            projectName = ((Entry)sender).Text;
        }

        public void CollectionDate_DateSelected(object sender, EventArgs e)
        {
            collectionDate = ((DatePicker)sender).Date; // maybe this will work?
            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/datepicker
        }

        public void GroupPhoto_OnClick(object sender, EventArgs e)
        {
            // open camera
            // popup photo save, or just save to DB, or setting for this?
        }

        public async void SaveTrip_OnClick(object sender, EventArgs e)
        {
            // save trip data
            // go to site page?
            // await Navigation.PushModalAsync(new SitePage());
            trip.AdditionalCollectors = additionalCollectors;
            trip.CollectionDate = collectionDate;
            trip.PrimaryCollector = primaryCollector;
            trip.Sites = new List<Site>();
        }

        public async void NewSite_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SitePage(trip));
        }
    }
}