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

        // default constructor for xaml preview
        public TripPage() { }

        // constructor for collecting
        public TripPage(Project project)
        {
            this.project = project;
            projectName = project.ProjectName;
            trip = new Trip();

            InitializeComponent();

            List<Trip> trips = ORM.GetTrips(projectName);
            entryTripName.Text = projectName + " - Trip" + (trips.Count + 1).ToString();
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

        public async void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            if (editing) // editing should only require all existing information to be changed
            {
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

            bool defaultData = false;

            // check to make sure name is present
            if (entryTripName.Text is null || entryAdditionalCollectors.Text is null)
            {
                trip.TripName = entryTripName.Text; // only trip name is required
                // add default date
                trip.CollectionDate = DateTime.Today;
                defaultData = true;
            }

            if (!defaultData)
            {
                trip.TripName = entryTripName.Text;
                trip.AdditionalCollectors = entryAdditionalCollectors.Text;
                trip.CollectionDate = dpCollectionDate.Date;
            }

            // check for duplicate names before saving
            if (ORM.CheckExists(trip))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a trip with the same name!");
                return;
            }

            // save trip to database
            int autoKeyResult = ORM.GetConnection().Insert(trip);
            Debug.WriteLine("inserted trip, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved Trip " + trip.TripName);

            // automatically navigate to Site page after saving Trip
            await Navigation.PushAsync(new SitePage(trip));
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