using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDSkeleton
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CollectingPage : TabbedPage
	{
        private Project project;
        private Trip trip;
        private Site site;
        private Specimen specimen;

        // overloaded constructor that accepts a Project as an argument
        // this should be passed in MainPage once the Project is chosen from the prompt
        public CollectingPage (Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        /*
         * Trip Section
         *
         */
        private string tripName = "";
        private string additionalCollectors = "";
        private DateTime tripDate;
        private bool dateSelected = false;

        public void entryTripName_Completed(object sender, EventArgs e)
        {
            tripName = entryTripName.Text;
        }

        public void entryAdditionalCollectors_Completed(object sender, EventArgs e)
        {
            additionalCollectors = entryTripName.Text;
        }

        public void dpCollectionDate_DateSelected(object sender, EventArgs e)
        {
            tripDate = dpCollectionDate.Date;
            dateSelected = true;
        }

        public async void btnGroupPhotoTrip_Clicked(object sender, EventArgs e)
        {
            if (project.ProjectName.Equals("") || trip.TripName.Equals(""))
            {
                // need trip name first
                return;
            }
            await TakePhoto.CallCamera(project.ProjectName + "-" + trip.TripName);             
        }

        public void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();
            trip.TripName = (tripName.Equals("")) ? entryTripName.Text : tripName;
            trip.AdditionalCollectors = (additionalCollectors.Equals("")) ? entryTripName.Text : additionalCollectors;
            trip.CollectionDate = (dateSelected) ? tripDate : DateTime.Today;

            trip.ProjectName = project.ProjectName;

            if (trip.TripName.Equals("") || trip.AdditionalCollectors.Equals(""))
            {
                // toast missing info
                return;    
            }

            ORM.GetConnection().CreateTable<Trip>();
            int autoKeyResult = ORM.GetConnection().Insert(trip);
            // toast saved trip
            Debug.WriteLine("inserted trip, recordno is: " + autoKeyResult.ToString());
        }

        public void btnNewTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();
            // reset all controls
            tripName = "";
            additionalCollectors = "";
            dateSelected = false;
            entryTripName.Text = "";
            entryAdditionalCollectors.Text = "";
            dpCollectionDate.Date = DateTime.Today;
        }

        /*
         * Site Section
         * 
         */
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

            if (site.SiteName.Equals("") || site.Locality.Equals("") || site.Habitat.Equals("") || site.AssociatedTaxa.Equals("") || site.LocationNotes.Equals(""))
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
        }

        /*
         * Specimen Section
         * 
         */
        private string fieldID = "";
        private string occurrenceNotes = "";
        private string substrate = "";
        private string lifeStage = "";
        private bool cultivated = false;
        private string individualCount = "";
        private string specimenGPS = "";

        public void entryFieldID_Completed(object sender, EventArgs e)
        {
            fieldID = entryFieldID.Text;
        }

        public void entryOccurrenceNotes_Completed(object sender, EventArgs e)
        {
            occurrenceNotes = entryOccurrenceNotes.Text;
        }

        public void entrySubstrate_Completed(object sender, EventArgs e)
        {
            substrate = entrySubstrate.Text;
        }

        public void pickerLifeStage_SelectedIndexChange(object sender, EventArgs e)
        {
            lifeStage = pickerLifeStage.SelectedItem.ToString();
        }

        public void switchCultivated_Toggled(object sender, EventArgs e)
        {
            cultivated = switchCultivated.IsToggled;
        }

        public void entryIndivCount_Completed(object sender, EventArgs e)
        {
            individualCount = entryIndivCount.Text;
        }

        public async void btnSetSpecimenPhoto_Clicked(object sender, EventArgs e)
        {
            // get specimen name first
            if (site.SiteName.Equals("") || specimen.FieldIdentification.Equals(""))
            {
                // toast need specimen id
                return;
            }
            await TakePhoto.CallCamera(site.SiteName + "-" + specimen.FieldIdentification);
        }

        public void btnSaveSpecimen_Clicked(object sender, EventArgs e)
        {
            specimen = new Specimen();
            specimen.FieldIdentification = (fieldID.Equals("")) ? entryFieldID.Text : fieldID;
            specimen.OccurrenceNotes = (occurrenceNotes.Equals("")) ? entryOccurrenceNotes.Text : occurrenceNotes;
            specimen.Substrate = (substrate.Equals("")) ? entrySubstrate.Text : substrate;
            specimen.LifeStage = (lifeStage.Equals("")) ? pickerLifeStage.SelectedItem.ToString() : lifeStage;
            specimen.Cultivated = (cultivated == false) ? switchCultivated.IsToggled : cultivated;
            specimen.IndividualCount = (individualCount.Equals("")) ? entryIndivCount.Text : individualCount;
            specimen.GPSCoordinates = specimenGPS;

            specimen.SiteName = site.SiteName;

            if (specimen.FieldIdentification.Equals("") || specimen.OccurrenceNotes.Equals("") || specimen.Substrate.Equals("") || specimen.LifeStage.Equals("") || specimen.IndividualCount.Equals(""))
            {
                // toast need all info
                return;
            }

            ORM.GetConnection().CreateTable<Specimen>();
            int autoKeyResult = ORM.GetConnection().Insert(specimen);
            // toast saved trip
            Debug.WriteLine("inserted specimen, recordno is: " + autoKeyResult.ToString());
        }

        public async void btnSetSpecimenGPS_Clicked(object sender, EventArgs e)
        {
            specimenGPS = await CurrentGPS.CurrentLocation();
        }

        public void btnNewSpecimen_Clicked(object sender, EventArgs e)
        {
            specimen = new Specimen();
        }
	}
}