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

        private bool editing = false;
        private bool dateChanged = false;

        // default constructor for xaml preview
        public TripPage() { }

        // constructor for collecting
        public TripPage(Project project)
        {
            this.project = project;
            trip = new Trip();
            InitializeComponent();
            LoadDefaults();
        }

        private void LoadDefaults()
        {
            entryTripName.Text = "Trip-" + (ORM.GetAllSitesCount() + 1).ToString();
            dpCollectionDate.Date = DateTime.Today;
        }

        // constructor for editing
        public TripPage(Trip trip)
        {
            this.trip = trip;
            editing = true;
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
            dateChanged = true;
        }

        // may get rid if this button
        //public async void btnGroupPhotoTrip_Clicked(object sender, EventArgs e)
        //{
        //    if (entryTripName.Text == null || entryTripName.Text.Equals(""))
        //    {
        //        DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a Trip name before taking photo");
        //        return;
        //    }

        //    await TakePhoto.CallCamera(project.ProjectName + "-" + trip.TripName);
        //}
        // xaml code
        //<Button x:Name="btnGroupPhotoTrip"
        //Text="Group Photo"
        //Clicked="btnGroupPhotoTrip_Clicked" />

        public async void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            if (editing)
            {
                trip.AdditionalCollectors = (entryAdditionalCollectors.Text is null) ? "" : entryAdditionalCollectors.Text;
                trip.CollectionDate = dateChanged ? dpCollectionDate.Date : trip.CollectionDate;

                int updateResult = ORM.GetConnection().Update(trip, typeof(Trip));
                if (updateResult == 1)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(trip.TripName + " update succeeded.");
                    return;
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(trip.TripName + " update failed.");
                    return;
                }
            }

            // check to make sure name is present
            if (entryTripName.Text is null || entryTripName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Trip name is required.");
                return;
            }

            trip.ProjectName = project.ProjectName;
            trip.TripName = entryTripName.Text;
            trip.AdditionalCollectors = (entryAdditionalCollectors.Text is null) ? "" : entryAdditionalCollectors.Text;
            trip.CollectionDate = dpCollectionDate.Date;

            // check for duplicate names before saving
            if (ORM.CheckExists(trip))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a trip with the same name!");
                return;
            }

            // save trip to database
            int autoKeyResult = ORM.GetConnection().Insert(trip);
            Debug.WriteLine("inserted trip, recordno is: " + autoKeyResult.ToString());

            // DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved Trip " + trip.TripName);

            // automatically navigate to Site page after saving Trip
            await Navigation.PushAsync(new SitePage(trip));
        }

        public void btnNewTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();

            entryTripName.Text = "";
            entryAdditionalCollectors.Text = "";
            trip.ProjectName = project.ProjectName;

            LoadDefaults();

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Trip");
        }
    }
}