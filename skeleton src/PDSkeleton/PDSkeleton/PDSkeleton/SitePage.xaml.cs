using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PDSkeleton
{
    public partial class SitePage : ContentPage
    {
        private Trip trip;
        private Site site;

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

        public void entrySiteName_Completed(object sender, EventArgs e)
        {
            siteName = entrySiteName.Text;
        }

        public void entryLocality_Completed(object sender, EventArgs e)
        {
            locality = entryLocality.Text;
        }

        public void entryHabitat_Completed(object sender, EventArgs e)
        {
            habitat = entryHabitat.Text;
        }

        public void entryAssocTaxa_Completed(object sender, EventArgs e)
        {
            associatedTaxa = entryAssocTaxa.Text;
        }

        public void entryLocationNotes_Completed(object sender, EventArgs e)
        {
            locationNotes = entryLocationNotes.Text;
        }

        public async void btnSitePhoto_Clicked(object sender, EventArgs e)
        {
            // get site name
            if (trip.TripName.Equals("") || site.SiteName.Equals(""))
            {
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

            site.TripName = trip.TripName;

            if (site.SiteName == null || site.Locality == null || site.Habitat == null || site.AssociatedTaxa == null || site.LocationNotes == null)
            {
                // toast need all info
                return;
            }

            ORM.GetConnection().CreateTable<Site>();
            int autoKeyResult = ORM.GetConnection().Insert(site);
            // toast saved trip
            Debug.WriteLine("inserted site, recordno is: " + autoKeyResult.ToString());
        }

        public async void btnSetSiteGPS_Clicked(object sender, EventArgs e)
        {
            siteGPS = await CurrentGPS.CurrentLocation();
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
        }

    }
}
