using System;
using System.Collections.Generic;
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
        public void entryTripName_Completed(object sender, EventArgs e)
        {
            
        }

        public void entryAdditionalCollectors_Completed(object sender, EventArgs e)
        {
            
        }

        public void dpCollectionDate_DateSelected(object sender, EventArgs e)
        {
            
        }

        public async void btnGroupPhotoTrip_Clicked(object sender, EventArgs e)
        {
            await TakePhoto.CallCamera(project.ProjectName + "-" + trip.TripName);
        }

        public void btnSaveTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();
        }

        public void btnNewTrip_Clicked(object sender, EventArgs e)
        {
            trip = new Trip();
            // clear fields
        }

        /*
         * Site Section
         * 
         */ 

        public void entryLocality_Completed(object sender, EventArgs e)
        {
            
        }

        public void entryHabitat_Completed(object sender, EventArgs e)
        {
            
        }

        public void entryAssocTaxa_Completed(object sender, EventArgs e)
        {
            
        }

        public void entryLocationNotes_Completed(object sender, EventArgs e)
        {
            
        }

        public async void btnSitePhoto_Clicked(object sender, EventArgs e)
        {
            // get site name
            await TakePhoto.CallCamera(site.SiteName);
        }

        public void btnSaveSite_Clicked(object sender, EventArgs e)
        {
            
        }

        public void btnSetSiteGPS_Clicked(object sender, EventArgs e)
        {
            
        }

        public void btnNewSite_Clicked(object sender, EventArgs e)
        {
            
        }

        /*
         * Specimen Section
         * 
         */
        public void entryFieldID_Completed(object sender, EventArgs e)
        {
            
        }

        public void entryOccurrenceNotes_Completed(object sender, EventArgs e)
        {
            
        }

        public void entrySubstrate_Completed(object sender, EventArgs e)
        {
            
        }

        public void pickerLifeStage_SelectedIndexChange(object sender, EventArgs e)
        {
            
        }

        public void switchCultivated_Toggled(object sender, EventArgs e)
        {
            
        }

        public void entryIndivCount_Completed(object sender, EventArgs e)
        {
            
        }

        public async void btnSetSpecimenPhoto_Clicked(object sender, EventArgs e)
        {
            // get specimen name first
            await TakePhoto.CallCamera(site.SiteName + "-" + specimen.FieldIdentification);
        }

        public void btnSaveSpecimen_Clicked(object sender, EventArgs e)
        {
            
        }

        public void btnSetSpecimenGPS_Clicked(object sender, EventArgs e)
        {
            
        }

        public void btnNewSpecimen_Clicked(object sender, EventArgs e)
        {
            
        }
	}
}