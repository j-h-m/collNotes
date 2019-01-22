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

        private string siteGPS = "";
        private bool editing = false;
        private string tripName = "";

        // default constructor for xaml preview
        public SitePage() { }

        // constructor accepts Trip as argument
        // Site is then added with this Trip's name for later association
        public SitePage(Trip trip)
        {
            site = new Site();
            this.trip = trip;
            tripName = trip.TripName;
            InitializeComponent();
        }

        public SitePage(Site site)
        {
            this.site = site;
            tripName = site.TripName;
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
            if (entrySiteName.Text == null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a Site name before taking photo");
                return;
            }

            if (tripName.Equals("") || entrySiteName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Select a Site and name the Site before taking photo");
                return;
            }
            await TakePhoto.CallCamera(tripName + "-" + entrySiteName.Text);
        }

        public void btnSaveSite_Clicked(object sender, EventArgs e)
        {
            if (editing) // editing should only require all existing information to be changed
            {
                if (!entryHabitat.Text.Equals("") && !entryLocality.Text.Equals("") && !entryAssocTaxa.Text.Equals("") && entryLocationNotes.Text.Equals("") &&
                   !(entryHabitat.Text is null) && !(entryLocality.Text is null) && !(entryAssocTaxa.Text is null) && !(entryLocationNotes.Text is null))
                {
                    site.AssociatedTaxa = entryAssocTaxa.Text;
                    site.GPSCoordinates = (siteGPS.Equals("")) ? site.GPSCoordinates : siteGPS;
                    site.Habitat = entryHabitat.Text;
                    site.Locality = entryLocality.Text;
                    site.LocationNotes = entryLocationNotes.Text;

                    int updateResult = ORM.GetConnection().Update(site, typeof(Site));
                    if (updateResult == 1)
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(site.SiteName + " save succeeded.");
                        return;
                    }
                    else
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(site.SiteName + " save failed.");
                        return;
                    }
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info to save Site!");
                    return;
                }
            }

            site.GPSCoordinates = siteGPS;
            site.TripName = tripName;

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
            existingSites = ORM.GetSites(site.TripName);

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
            lblStatusMessage.IsVisible = true;
            lblStatusMessage.TextColor = Color.Orange;
            lblStatusMessage.Text = "Getting Location...";

            siteGPS = await CurrentGPS.CurrentLocation();

            if (siteGPS.Equals(""))
            {
                lblStatusMessage.TextColor = Color.Red;
                lblStatusMessage.Text = "Failed to get location";
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Failed to get GPS location. Is Location enabled?");
            }
            else
            {
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

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Site");
        }
    }
}
