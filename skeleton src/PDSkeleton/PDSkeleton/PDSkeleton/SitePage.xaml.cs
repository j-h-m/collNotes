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
        private List<Site> existingSites;

        // constructor accepts Trip as argument
        // Site is then added with this Trip's name for later association
        public SitePage(Trip trip)
        {
            this.trip = trip;
            InitializeComponent();
        }
        private string siteGPS = "";

        // Site Name text entry event
        public void entrySiteName_Completed(object sender, EventArgs e)
        {
            site.SiteName = entrySiteName.Text;
        }

        // locality text entry event
        public void entryLocality_Completed(object sender, EventArgs e)
        {
            site.Locality = entryLocality.Text;
        }

        // habitat text entry event
        public void entryHabitat_Completed(object sender, EventArgs e)
        {
            site.Habitat = entryHabitat.Text;
        }

        // associated taxa text entry event
        public void entryAssocTaxa_Completed(object sender, EventArgs e)
        {
            site.AssociatedTaxa = entryAssocTaxa.Text;
        }

        // location notes text entry event
        public void entryLocationNotes_Completed(object sender, EventArgs e)
        {
            site.LocationNotes = entryLocationNotes.Text;
        }

        // site photo button event
        public async void btnSitePhoto_Clicked(object sender, EventArgs e)
        {
            // get site name
            if (entrySiteName.Text == null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a Site name before taking photo");
                return;
            }

            if (trip.TripName.Equals("") || entrySiteName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Select a Site and name the Site before taking photo");
                return;
            }
            await TakePhoto.CallCamera(trip.TripName + "-" + entrySiteName.Text);
        }

        public void btnSaveSite_Clicked(object sender, EventArgs e)
        {
            site = new Site();

            site.GPSCoordinates = siteGPS;
            site.TripName = trip.TripName;

            // check to make sure all data is present
            if (entrySiteName.Text is null || entryLocality.Text is null || entryHabitat.Text is null || entryAssocTaxa.Text is null || entryLocationNotes.Text is null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info to save Site");
                return;
            }

            site.SiteName = entrySiteName.Text;
            site.Locality = entryLocality.Text;
            site.Habitat = entryHabitat.Text;
            site.AssociatedTaxa = entryAssocTaxa.Text;
            site.LocationNotes = entryLocationNotes.Text;

            ORM.GetConnection().CreateTable<Site>();

            // check for duplicate names first
            existingSites = ORM.GetConnection().Query<Site>("select * from Site");

            foreach (Site s in existingSites)
            {
                if (s.SiteName.Equals(site.SiteName))
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a site with the same name!");
                    return;
                }
            }
            // save site to database
            int autoKeyResult = ORM.GetConnection().Insert(site);
            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Site " + site.SiteName + " saved!");
            Debug.WriteLine("inserted site, recordno is: " + autoKeyResult.ToString());
        }

        public async void btnSetSiteGPS_Clicked(object sender, EventArgs e)
        {
            siteGPS = await CurrentGPS.CurrentLocation();
            lblStatusMessage.IsVisible = true;
            lblStatusMessage.TextColor = Color.Orange;
            lblStatusMessage.Text = "Getting Location...";

            if (siteGPS.Equals(""))
            {
                lblStatusMessage.IsVisible = true;
                lblStatusMessage.TextColor = Color.Red;
                lblStatusMessage.Text = "Failed to get location";
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Failed to get GPS location. Is Location enabled?");
            }
            else
            {
                lblStatusMessage.IsVisible = true;
                lblStatusMessage.TextColor = Color.Blue;
                lblStatusMessage.Text = "Location: " + siteGPS;
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Location: " + siteGPS);
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

            lblStatusMessage.IsVisible = false;
            lblStatusMessage.Text = "";

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared data for new Site");
        }

    }
}
