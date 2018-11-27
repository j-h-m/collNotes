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
            if (editing)
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

            // check to make sure all data is present
            if (entrySiteName.Text is null || entryLocality.Text is null || entryHabitat.Text is null || entryAssocTaxa.Text is null || entryLocationNotes.Text is null ||
               entrySiteName.Text.Equals("") || entryLocality.Text.Equals("") || entryHabitat.Text.Equals("") || entryAssocTaxa.Text.Equals("") || entryLocationNotes.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter all information for Site!");
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
            lblStatusMessage.IsVisible = true;
            lblStatusMessage.TextColor = Color.Orange;
            lblStatusMessage.Text = "Getting Location...";

            pbProgressStatus.IsVisible = true;
            await pbProgressStatus.ProgressTo(1.0, 5, Easing.Linear);

            siteGPS = await CurrentGPS.CurrentLocation();

            if (siteGPS.Equals(""))
            {
                pbProgressStatus.IsVisible = false;
                lblStatusMessage.TextColor = Color.Red;
                lblStatusMessage.Text = "Failed to get location";
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Failed to get GPS location. Is Location enabled?");
            }
            else
            {
                pbProgressStatus.IsVisible = false;
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
