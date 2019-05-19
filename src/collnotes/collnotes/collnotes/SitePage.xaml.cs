using System;
using System.Diagnostics;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;
using collnotes.Plugins;

namespace collnotes
{
    public partial class SitePage : ContentPage
    {
        // hold object instance(s) for current Page
        private Trip trip;
        private Site site;

        // flags used to control what happens in some events
        private bool userIsEditing = false;
        private bool editWasSaved = false;

        // field for the user's output from call to GPS
        private string siteGPS = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.SitePage"/> class.
        /// Empty constructor, is only used by the Device Preview feature of Visual Studio.
        /// </summary>
        public SitePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.SitePage"/> class.
        /// Create Site constructor. Takes the Trip to create Site under as an argument.
        /// </summary>
        /// <param name="trip">Trip.</param>
        public SitePage(Trip trip)
        {
            site = new Site();
            this.trip = trip;
            InitializeComponent();
            LoadDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.SitePage"/> class.
        /// Edit Site constructor. Takes the Site to edit as an argument.
        /// </summary>
        /// <param name="site">Site.</param>
        public SitePage(Site site)
        {
            this.site = site;
            InitializeComponent();

            Title = site.RecordNo.ToString() + "-#";

            entrySiteName.Text = site.SiteName;
            entrySiteName.IsEnabled = false;
            btnNewSite.IsVisible = false;
            entryLocality.Text = site.Locality;
            entryHabitat.Text = site.Habitat;
            entryAssocTaxa.Text = site.AssociatedTaxa;
            entryLocationNotes.Text = site.LocationNotes;

            siteGPS = site.GPSCoordinates;

            userIsEditing = true;
            editWasSaved = false;

            // Hide the back button and only show Update/Cancel buttons for user
            NavigationPage.SetHasBackButton(this, false);
            btnBack.IsVisible = userIsEditing; // by default the button is visible, this is here for clarity

            // look at the current GPS and set the lblStatusMessage.Text property
            // the lblStatusMessage.TextColor property is modified to reflect accuracy
            try
            {
                lblStatusMessage.Text = "Current GPS: " + site.GPSCoordinates;
                string[] splitGPS = site.GPSCoordinates.Split(',');
                var accuracy = Convert.ToDouble(splitGPS[2]);
                if (accuracy >= 0 && accuracy <= 20)
                {
                    lblStatusMessage.TextColor = Color.Green;
                }
                else if (accuracy >= 21 && accuracy <= 30)
                {
                    lblStatusMessage.TextColor = Color.Yellow;
                }
                else // 31 or greater
                {
                    lblStatusMessage.TextColor = Color.Orange;
                }
            }
            // on any error we assume the GPS was not recorded
            // i know this is a big assumption, but it just depends on splitting a string that only gets saved one way...
            catch
            {
                lblStatusMessage.Text = "GPS was not recorded!";
                lblStatusMessage.TextColor = Color.Red;
            }
        }

        /// <summary>
        /// Loads the defaults.
        /// </summary>
        private void LoadDefaults()
        {
            entrySiteName.Text = "Site-" + (DataFunctions.GetAllSitesCount() + 1).ToString();
            Title = (DataFunctions.GetAllSitesCount() + 1).ToString() + "-#";
            btnBack.IsVisible = userIsEditing;
        }

        /// <summary>
        /// btnSitePhoto Click event.
        /// Allows the user to take a photo by using the TakePhoto.CallCamera function.
        /// Requires the entrySiteName field to be not null or a non-empty string.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void SitePhoto_Clicked(object sender, EventArgs e)
        {
            // get site name
            if (entrySiteName.Text is null || entrySiteName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Name the Site before taking photo.");
                return;
            }

            await TakePhoto.CallCamera(trip.TripName + "-" + entrySiteName.Text);
        }

        /// <summary>
        /// btnSaveSite Click event.
        /// Handles Save/Update of the current Site.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void SaveSite_Clicked(object sender, EventArgs e)
        {
            if (userIsEditing)
            {
                site.AssociatedTaxa = (entryAssocTaxa.Text is null) ? "" : entryAssocTaxa.Text;
                site.GPSCoordinates = siteGPS.Equals("") ? site.GPSCoordinates : siteGPS;
                site.Habitat = (entryHabitat.Text is null) ? "" : entryHabitat.Text;
                site.Locality = (entryLocality.Text is null) ? "" : entryLocality.Text;
                site.LocationNotes = (entryLocationNotes.Text is null) ? "" : entryLocationNotes.Text;

                int updateResult = DatabaseFile.GetConnection().Update(site, typeof(Site));
                if (updateResult == 1)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(site.SiteName + " update succeeded.");
                    editWasSaved = true;
                    return;
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(site.SiteName + " update failed.");
                    return;
                }
            }

            SaveCurrentSite();

            // automatically navigate to the specimen page after creating the site
            await Navigation.PushAsync(new SpecimenPage(site));
            Navigation.RemovePage(this);
        }

        /// <summary>
        /// btnSetSiteGPS Click event.
        /// Makes an async call to the CurrentGPS.CurrentLocation function.
        /// On success it sets the User's current location, on fail it does not.
        /// In either situation the User will be notified.
        /// Note: This is the second most important function in collNotes, as the GPS coordinates serve a purpose later on.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void SetSiteGPS_Clicked(object sender, EventArgs e)
        {
            lblStatusMessage.IsVisible = true;
            lblStatusMessage.TextColor = Color.Orange;
            lblStatusMessage.Text = "Getting Location...";

            Plugin.Geolocator.Abstractions.Position currentPosition;
            currentPosition = await CurrentGPS.CurrentLocation();

            if (currentPosition is null)
            {
                lblStatusMessage.TextColor = Color.Red;
                lblStatusMessage.Text = "Failed to get location";
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("collNotes requires Location permission, and Location must be enabled!");
            }
            else
            {
                // 0 - 20    good
                if (currentPosition.Accuracy >= 0 && currentPosition.Accuracy <= 20)
                {
                    lblStatusMessage.TextColor = Color.Green;
                }
                // 21 - 30   ok
                else if (currentPosition.Accuracy >= 21 && currentPosition.Accuracy <= 30)
                {
                    lblStatusMessage.TextColor = Color.Yellow;
                }
                // 31 - ...  poor
                else
                {
                    lblStatusMessage.TextColor = Color.Orange;
                }

                lblStatusMessage.Text = "Location Received";
                siteGPS = currentPosition.Latitude.ToString() + "," + currentPosition.Longitude.ToString() + "," + currentPosition.Accuracy.ToString() + "," + currentPosition.Altitude.ToString();

            }
        }

        /// <summary>
        /// btnNewSite Click event.
        /// Clears the Page for a new Site.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void NewSite_Clicked(object sender, EventArgs e)
        {
            site = new Site();

            entrySiteName.Text = "";
            entryLocality.Text = "";
            entryHabitat.Text = "";
            entryAssocTaxa.Text = "";
            entryLocationNotes.Text = "";
            siteGPS = "";
            lblStatusMessage.IsVisible = false;
            lblStatusMessage.Text = "";

            site.TripName = trip.TripName;

            LoadDefaults();
        }

        /// <summary>
        /// btnBack Click event.
        /// Provides the user with the option to exit an Update page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private async void Back_Clicked(object sender, EventArgs e)
        {
            if (editWasSaved)
            {
                Navigation.RemovePage(this);
                return;
            }

            if (entryHabitat.Text.Equals(site.Habitat) &&
                entryLocality.Text.Equals(site.Locality) &&
                entryAssocTaxa.Text.Equals(site.AssociatedTaxa) &&
                entryLocationNotes.Text.Equals(site.LocationNotes) &&
                site.GPSCoordinates.Equals(siteGPS))
            {
                Navigation.RemovePage(this);
                return;
            }

            bool response = await DisplayAlert("Confirm", "Discard changes?", "Yes", "No");
            if (response)
                Navigation.RemovePage(this);
        }

        /// <summary>
        /// Saves the current site.
        /// </summary>
        /// <returns><c>true</c>, if current site was saved, <c>false</c> otherwise.</returns>
        private bool SaveCurrentSite()
        {
            if (siteGPS.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Record the Site GPS first!");
                return false;
            }

            // saving new Site
            site.GPSCoordinates = siteGPS;
            site.TripName = trip.TripName;

            // only require name to save Site
            if (entrySiteName.Text is null || entrySiteName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter a name for Site!");
                return false;
            }

            site.SiteName = entrySiteName.Text;

            site.Locality = entryLocality.Text is null ? "" : entryLocality.Text;
            site.Habitat = entryHabitat.Text is null ? "" : entryHabitat.Text;
            site.AssociatedTaxa = entryAssocTaxa.Text is null ? "" : entryAssocTaxa.Text;
            site.LocationNotes = entryLocationNotes.Text is null ? "" : entryLocationNotes.Text;

            // check for duplicate names before saving
            if (DataFunctions.CheckExists(site, site.SiteName))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a site with the same name!");
                return false;
            }

            // save site to database
            int autoKeyResult = DatabaseFile.GetConnection().Insert(site);
            // DependencyService.Get<ICrossPlatformToast>().ShortAlert("Site " + site.SiteName + " saved!");
            Debug.WriteLine("inserted site, recordno is: " + autoKeyResult.ToString());

            return true;
        }

        /// <summary>
        /// override the OnBackButtonPressed event in Xamarin Forms.
        /// this event fires when the user clicks the hardware back button on Android phones.
        /// we just disable the back button behavior when the user is editing
        /// </summary>
        /// <returns>bool</returns>
        protected override bool OnBackButtonPressed()
        {
            // return value:
            // false - do back button behavior
            // true  - don't
            if (userIsEditing)
            {
                // disable back button on android if user is editing
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
