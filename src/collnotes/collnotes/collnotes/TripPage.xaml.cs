using System;
using System.Diagnostics;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;

namespace collnotes
{
    /// <summary>
    /// Trip page.
    /// </summary>
    public partial class TripPage : ContentPage
    {
        // hold object instance(s) for current Page
        private Project project;
        private Trip trip;

        // flags used to control what happens in some events
        private bool userIsEditing = false;
        private bool dateChanged = false;
        private bool editWasSaved = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.TripPage"/> class.
        /// Empty constructor, is only used by the Device Preview feature of Visual Studio.
        /// </summary>
        public TripPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.TripPage"/> class.
        /// Create Trip constructor. Takes a Project as a parameter.
        /// The Trip will be created under the Project passed in.
        /// </summary>
        /// <param name="project">Project.</param>
        public TripPage(Project project)
        {
            this.project = project;
            trip = new Trip();
            InitializeComponent();
            LoadDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.TripPage"/> class.
        /// Edit Trip constructor. Takes the Trip to edit as an argument.
        /// </summary>
        /// <param name="trip">Trip.</param>
        public TripPage(Trip trip)
        {
            this.trip = trip;
            InitializeComponent();

            entryTripName.Text = trip.TripName;
            entryAdditionalCollectors.Text = trip.AdditionalCollectors;
            dpCollectionDate.Date = trip.CollectionDate;
            entryTripName.IsEnabled = false;
            btnNewTrip.IsVisible = false;

            userIsEditing = true;
            editWasSaved = false;

            // Hide the back button and only show Update/Cancel buttons for user
            NavigationPage.SetHasBackButton(this, false);
            btnBack.IsVisible = userIsEditing; // by default the button is visible, this is here for clarity
        }

        /// <summary>
        /// Loads the default values for controls with corresponding App Variables.
        /// </summary>
        private void LoadDefaults()
        {
            entryTripName.Text = "Trip-" + (DataFunctions.GetAllTripsCount() + 1).ToString();
            dpCollectionDate.Date = DateTime.Today;
            btnBack.IsVisible = userIsEditing;
        }

        /// <summary>
        /// dpCollectionDate DateSelected event.
        /// Sets the current Trip's CollectionDate.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void dpCollectionDate_DateSelected(object sender, EventArgs e)
        {
            trip.CollectionDate = dpCollectionDate.Date;
            dateChanged = true;
        }

        /// <summary>
        /// btnSaveTrip Click event.
        /// Handles the Saving, or Updating, of the current Trip.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            if (userIsEditing)
            {
                trip.AdditionalCollectors = (entryAdditionalCollectors.Text is null) ? "" : entryAdditionalCollectors.Text;
                trip.CollectionDate = dateChanged ? dpCollectionDate.Date : trip.CollectionDate;
                int updateResult = DatabaseFile.GetConnection().Update(trip, typeof(Trip));
                if (updateResult == 1)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(trip.TripName + " update succeeded.");
                    editWasSaved = true;
                    return;
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(trip.TripName + " update failed.");
                    return;
                }
            }

            SaveCurrentTrip();

            // automatically navigate to Site page after saving Trip
            await Navigation.PushAsync(new SitePage(trip));
            Navigation.RemovePage(this);
        }

        /// <summary>
        /// btnNewTrip Click event.
        /// Clears controls and loads defaults for a new Trip.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void btnNewTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();

            entryTripName.Text = "";
            entryAdditionalCollectors.Text = "";
            trip.ProjectName = project.ProjectName;
            LoadDefaults();

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Trip");
        }

        /// <summary>
        /// btnBack Click event.
        /// Provides the user with the option to exit an Update page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            if (editWasSaved)
            {
                Navigation.RemovePage(this);
                return;
            }

            if (entryAdditionalCollectors.Text.Equals(trip.AdditionalCollectors) &&
                dpCollectionDate.Date.ToShortDateString().Equals(trip.CollectionDate.ToShortDateString()))
            {
                Navigation.RemovePage(this);
                return;
            }

            bool response = await DisplayAlert("Confirm", "Discard changes?", "Yes", "No");
            if (response)
                Navigation.RemovePage(this);
        }

        /// <summary>
        /// Saves the current trip.
        /// </summary>
        /// <returns><c>true</c>, if current trip was saved, <c>false</c> otherwise.</returns>
        private bool SaveCurrentTrip()
        {
            // check to make sure name is present
            if (entryTripName.Text is null || entryTripName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Trip name is required.");
                return false;
            }

            trip.ProjectName = project.ProjectName;
            trip.TripName = entryTripName.Text;
            trip.AdditionalCollectors = (entryAdditionalCollectors.Text is null) ? "" : entryAdditionalCollectors.Text;
            trip.CollectionDate = dpCollectionDate.Date;

            // check for duplicate names before saving
            if (DataFunctions.CheckExists(trip, trip.TripName))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a trip with the same name!");
                return false;
            }

            // save trip to database
            int autoKeyResult = DatabaseFile.GetConnection().Insert(trip);
            Debug.WriteLine("inserted trip, recordno is: " + autoKeyResult.ToString());

            return true;
        }
    }
}
