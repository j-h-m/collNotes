using System;
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

        // constructor accepts Trip as argument
        // Site is then added with this Trip's name for later association
        public SitePage(Trip trip)
        {
            this.trip = trip;
            InitializeComponent();
        }

        private string siteName = "";
        private string locality = "";
        private string habitat = "";
        private string associatedTaxa = "";
        private string locationNotes = "";
        private string siteGPS = "";

        // Site Name text entry event
        public void entrySiteName_Completed(object sender, EventArgs e)
        {
            siteName = entrySiteName.Text;
        }

        // locality text entry event
        public void entryLocality_Completed(object sender, EventArgs e)
        {
            locality = entryLocality.Text;
        }

        // habitat text entry event
        public void entryHabitat_Completed(object sender, EventArgs e)
        {
            habitat = entryHabitat.Text;
        }

        // associated taxa text entry event
        public void entryAssocTaxa_Completed(object sender, EventArgs e)
        {
            associatedTaxa = entryAssocTaxa.Text;
        }

        // location notes text entry event
        public void entryLocationNotes_Completed(object sender, EventArgs e)
        {
            locationNotes = entryLocationNotes.Text;
        }

        // site photo button event
        public async void btnSitePhoto_Clicked(object sender, EventArgs e)
        {
            // get site name
            if (trip.TripName.Equals("") || entrySiteName.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Select a Trip and name the Site before taking photo");
                return;
            }
            await TakePhoto.CallCamera(trip.TripName + "-" + site.SiteName);
        }

        public void btnSaveSite_Clicked(object sender, EventArgs e)
        {
            site = new Site();
            site.SiteName = (siteName.Equals("")) ? entrySiteName.Text : siteName;
            site.Locality = (locality.Equals("")) ? entryLocality.Text : locality;
            site.Habitat = (habitat.Equals("")) ? entryHabitat.Text : habitat;
            site.AssociatedTaxa = (associatedTaxa.Equals("")) ? entryAssocTaxa.Text : associatedTaxa;
            site.LocationNotes = (locationNotes.Equals("")) ? entryLocationNotes.Text : locationNotes;
            site.GPSCoordinates = siteGPS;
            site.TripName = trip.TripName;

            if (site.SiteName == null || site.Locality == null || site.Habitat == null || site.AssociatedTaxa == null || site.LocationNotes == null)
            {
                // toast need all info
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info for Site");
                return;
            }

            ORM.GetConnection().CreateTable<Site>();
            int autoKeyResult = ORM.GetConnection().Insert(site);
            // toast saved trip
            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Site " + site.SiteName + " saved!");
            Debug.WriteLine("inserted site, recordno is: " + autoKeyResult.ToString());
        }

        public async void btnSetSiteGPS_Clicked(object sender, EventArgs e)
        {
            siteGPS = await CurrentGPS.CurrentLocation();
            if (siteGPS.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Failed to get GPS location. Is Location enabled?");
            }
            else
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Location: " + siteGPS);
            }
        }

        public void btnNewSite_Clicked(object sender, EventArgs e)
        {
            site = new Site();
            siteName = "";
            locality = "";
            habitat = "";
            associatedTaxa = "";
            locationNotes = "";

            entrySiteName.Text = "";
            entryLocality.Text = "";
            entryHabitat.Text = "";
            entryAssocTaxa.Text = "";
            entryLocationNotes.Text = "";

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared data for new Site");
        }

    }
}
