using System;
using System.Collections.Generic;
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
        private List<Trip> existingTrips;

        // constructor takes Project as argument
        public TripPage(Project project)
        {
            this.project = project;
            trip = new Trip();
            InitializeComponent();
        }

        // trip name text entry event
        public void entryTripName_Completed(object sender, EventArgs e)
        {
            trip.TripName = entryTripName.Text;
        }

        // additional collectors text entry event
        public void entryAdditionalCollectors_Completed(object sender, EventArgs e)
        {
            trip.AdditionalCollectors = entryAdditionalCollectors.Text;
        }

        // date picker collection date event
        public void dpCollectionDate_DateSelected(object sender, EventArgs e)
        {
            trip.CollectionDate = dpCollectionDate.Date;
        }

        public async void btnGroupPhotoTrip_Clicked(object sender, EventArgs e)
        {
            if (entryTripName.Text == null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a Trip name before taking photo");
                return;
            }

            if (project.ProjectName.Equals("") || entryTripName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a Trip name before taking photo");
                return;
            }
            await TakePhoto.CallCamera(project.ProjectName + "-" + trip.TripName);
        }

        public void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            trip.ProjectName = project.ProjectName;

            // check to make sure all data is present
            if (entryTripName.Text is null || entryAdditionalCollectors.Text is null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info to save Trip");
                return;
            }

            trip.TripName = entryTripName.Text;
            trip.AdditionalCollectors = entryAdditionalCollectors.Text;
            trip.CollectionDate = dpCollectionDate.Date;

            ORM.GetConnection().CreateTable<Trip>();

            // check for duplicate names first
            existingTrips = ORM.GetConnection().Query<Trip>("select * from Trip");

            foreach (Trip t in existingTrips)
            {
                if (t.TripName.Equals(trip.TripName))
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a trip with the same name!");
                    return;
                }
            }

            // save trip to database
            int autoKeyResult = ORM.GetConnection().Insert(trip);
            Debug.WriteLine("inserted trip, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved Trip " + trip.TripName);
        }

        public void btnNewTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();

            entryTripName.Text = "";
            entryAdditionalCollectors.Text = "";
            dpCollectionDate.Date = DateTime.Today;

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Trip");
        }
    }
}
