using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

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
        public Plugin.Media.Abstractions.MediaFile groupPhoto;

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

        public async void GroupPhoto_OnClick(object sender, EventArgs e)
        {
            groupPhoto = await TakePhoto.CallCamera();
        }

        public void SaveTrip_OnClick(object sender, EventArgs e)
        {
            trip.AdditionalCollectors = additionalCollectors;
            trip.CollectionDate = collectionDate;
            trip.PrimaryCollector = primaryCollector;
            trip.groupPhoto = this.groupPhoto;
            //trip.Sites = new List<Site>();

            // get new record no
            // get 

            SQLiteConnection conn = ORM.GetConnection();
            // how to get old table if one has already been created?
            conn.CreateTable<Trip>();
            conn.Insert(trip);

            Console.WriteLine("Reading data");
            var table = conn.Table<Trip>();
            foreach (var s in table)
            {
                Console.WriteLine(s.AdditionalCollectors + " " + s.PrimaryCollector);
            }
        }

        public async void NewSite_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SitePage(trip));
        }
    }
}