using System;
using System.Diagnostics;
using Xamarin.Forms;

/*
 * Trip Page
 * Create a new Trip
 */ 

namespace PDSkeleton
{
    public partial class TripPage : ContentPage
    {
        private Project project;
        private Trip trip;

        // constructor takes Project as argument
        public TripPage(Project project)
        {
            this.project = project;
            trip = new Trip();
            InitializeComponent();
        }

        private string tripName = "";
        private string additionalCollectors = "";
        private DateTime tripDate;
        private bool dateSelected = false;

        // trip name text entry event
        public void entryTripName_Completed(object sender, EventArgs e)
        {
            tripName = entryTripName.Text;
        }

        // additional collectors text entry event
        public void entryAdditionalCollectors_Completed(object sender, EventArgs e)
        {
            additionalCollectors = entryAdditionalCollectors.Text;
        }

        // date picker collection date event
        public void dpCollectionDate_DateSelected(object sender, EventArgs e)
        {
            tripDate = dpCollectionDate.Date;
            dateSelected = true;
        }

        public async void btnGroupPhotoTrip_Clicked(object sender, EventArgs e)
        {
            if (project.ProjectName.Equals("") || entryTripName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a Trip name before taking photo");
                return;
            }
            await TakePhoto.CallCamera(project.ProjectName + "-" + trip.TripName);
        }

        public void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            trip.TripName = (tripName.Equals("")) ? entryTripName.Text : tripName;
            trip.AdditionalCollectors = (additionalCollectors.Equals("")) ? entryTripName.Text : additionalCollectors;
            trip.CollectionDate = (dateSelected) ? tripDate : DateTime.Today;

            trip.ProjectName = project.ProjectName;

            if (trip.TripName == null || trip.AdditionalCollectors == null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info to save Trip");
                return;
            }

            ORM.GetConnection().CreateTable<Trip>();
            int autoKeyResult = ORM.GetConnection().Insert(trip);
            // toast saved trip
            Debug.WriteLine("inserted trip, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved Trip " + trip.TripName);
        }

        public void btnNewTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();
            // reset all controls
            tripName = "";
            additionalCollectors = "";
            dateSelected = false;

            entryTripName.Text = "";
            entryAdditionalCollectors.Text = "";
            dpCollectionDate.Date = DateTime.Today;

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Trip");
        }
    }
}
