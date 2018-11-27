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

        private bool editing = false;
        private string projectName = "";

        // constructor for collecting
        public TripPage(Project project)
        {
            this.project = project;
            projectName = project.ProjectName;
            trip = new Trip();
            InitializeComponent();
        }

        // constructor for editing
        public TripPage(Trip trip)
        {
            this.trip = trip;
            editing = true;
            projectName = trip.ProjectName;
            InitializeComponent();

            entryTripName.Text = trip.TripName;
            entryAdditionalCollectors.Text = trip.AdditionalCollectors;
            dpCollectionDate.Date = trip.CollectionDate;
            entryTripName.IsEnabled = false;
            btnNewTrip.IsEnabled = false;
        }

        // date picker collection date event
        public void dpCollectionDate_DateSelected(object sender, EventArgs e)
        {
            trip.CollectionDate = dpCollectionDate.Date;
        }

        public async void btnGroupPhotoTrip_Clicked(object sender, EventArgs e)
        {
            if (entryTripName.Text == null || projectName.Equals("") || entryTripName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a Trip name before taking photo");
                return;
            }

            await TakePhoto.CallCamera(projectName + "-" + trip.TripName);
        }

        public void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            if (editing)
            {
                // check for empty fields, required for an edit
                if (!entryAdditionalCollectors.Text.Equals("") && !entryAdditionalCollectors.Text.Equals(""))
                {
                    trip.AdditionalCollectors = entryAdditionalCollectors.Text;
                    trip.CollectionDate = dpCollectionDate.Date;

                    int updateResult = ORM.GetConnection().Update(trip, typeof(Trip));
                    if (updateResult == 1) // what is result of above call?
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(trip.TripName + " save succeeded.");
                        return;
                    }
                    else
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(trip.TripName + " save failed.");
                        return;
                    }
                }
            }

            trip.ProjectName = projectName;

            // check to make sure all data is present
            if (entryTripName.Text is null || entryAdditionalCollectors.Text is null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Adding default data for Trip");
                // add default data
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
