using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

/*
 * Site Page
 *  - users create a new Site under selected Trip
 */ 

namespace PDSkeleton
{
    public partial class SitePage : ContentPage
    {
        private Trip trip;
        private Site site;

        private string siteGPS = "";
        private bool editing = false;

        // default constructor for xaml preview
        public SitePage() { }

        // constructor accepts Trip as argument
        // Site is then added with this Trip's name for later association
        public SitePage(Trip trip)
        {
            site = new Site();
            this.trip = trip;
            InitializeComponent();
            LoadDefaults();
        }

        private void LoadDefaults()
        {
            entrySiteName.Text = "Site-" + (ORM.GetAllSitesCount() + 1).ToString();
        }

        public SitePage(Site site)
        {
            this.site = site;
            editing = true;
            InitializeComponent();

            entrySiteName.Text = site.SiteName;
            entrySiteName.IsEnabled = false;
            btnNewSite.IsEnabled = false;
            entryLocality.Text = site.Locality;
            entryHabitat.Text = site.Habitat;
            entryAssocTaxa.Text = site.AssociatedTaxa;
            entryLocationNotes.Text = site.LocationNotes;
        }

        // site photo button event
        public async void btnSitePhoto_Clicked(object sender, EventArgs e)
        {
            // get site name
            if (entrySiteName.Text is null || entrySiteName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Name the Site before taking photo.");
                return;
            }

            await TakePhoto.CallCamera(trip.TripName + "-" + entrySiteName.Text);
        }

        public async void btnSaveSite_Clicked(object sender, EventArgs e)
        {
            if (editing) // editing should only require all existing information to be changed
            {
                site.AssociatedTaxa = (entryAssocTaxa.Text is null) ? "" : entryAssocTaxa.Text;
                site.GPSCoordinates = siteGPS.Equals("") ? site.GPSCoordinates : siteGPS;
                site.Habitat = (entryHabitat.Text is null) ? "" : entryHabitat.Text;
                site.Locality = (entryLocality.Text is null) ? "" : entryLocality.Text;
                site.LocationNotes = (entryLocationNotes.Text is null) ? "" : entryLocationNotes.Text;

                int updateResult = ORM.GetConnection().Update(site, typeof(Site));
                if (updateResult == 1)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(site.SiteName + " update succeeded.");
                    return;
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(site.SiteName + " update failed.");
                    return;
                }
            }

            if (siteGPS.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Record the Site GPS first!");
                return;
            }

            // saving new Site
            site.GPSCoordinates = siteGPS;
            site.TripName = trip.TripName;

            // only require name to save Site
            if (entrySiteName.Text is null || entrySiteName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter a name for Site!");
                return;
            }

            site.SiteName = entrySiteName.Text;

            site.Locality = entryLocality.Text is null ? "" : entryLocality.Text;
            site.Habitat = entryHabitat.Text is null ? "" : entryHabitat.Text;
            site.AssociatedTaxa = entryAssocTaxa.Text is null ? "" : entryAssocTaxa.Text;
            site.LocationNotes = entryLocationNotes.Text is null ? "" : entryLocationNotes.Text;

            // check for duplicate names before saving
            if (ORM.CheckExists(site))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a site with the same name!");
                return;
            }

            // save site to database
            int autoKeyResult = ORM.GetConnection().Insert(site);
            // DependencyService.Get<ICrossPlatformToast>().ShortAlert("Site " + site.SiteName + " saved!");
            Debug.WriteLine("inserted site, recordno is: " + autoKeyResult.ToString());
            // automatically navigate to the specimen page after creating the site
            await Navigation.PushAsync(new SpecimenPage(site));
        }

        public async void btnSetSiteGPS_Clicked(object sender, EventArgs e)
        {
            lblStatusMessage.IsVisible = true;
            lblStatusMessage.TextColor = Color.Orange;
            lblStatusMessage.Text = "Getting Location...";

            siteGPS = await CurrentGPS.CurrentLocation();

            if (siteGPS.Equals(""))
            {
                lblStatusMessage.TextColor = Color.Red;
                lblStatusMessage.Text = "Failed to get location";
            }
            else
            {
                lblStatusMessage.TextColor = Color.Green;
                lblStatusMessage.Text = "Location Received";
            }
        }

        public void btnNewSite_Clicked(object sender, EventArgs e)
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

            LoadDefaults();

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Site");
        }
    }
}
