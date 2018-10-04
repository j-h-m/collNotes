using System;
using System.Diagnostics;
using Xamarin.Forms;

/*
 * Specimen Page
 * Create a new Specimen
 */ 

namespace PDSkeleton
{
    public partial class SpecimenPage : ContentPage
    {
        private Site site;
        private Specimen specimen;
        private string fieldID = "";
        private string occurrenceNotes = "";
        private string substrate = "";
        private string lifeStage = "";
        private bool cultivated = false;
        private string individualCount = "";
        private string specimenGPS = "";

        public SpecimenPage(Site site)
        {
            this.site = site;
            InitializeComponent();
        }

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

            if (specimen.FieldIdentification == null || specimen.OccurrenceNotes == null || specimen.Substrate == null || specimen.LifeStage == null || specimen.IndividualCount == null)
            {
                // toast need all info
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all Specimen info to save it");
                return;
            }

            ORM.GetConnection().CreateTable<Specimen>();
            int autoKeyResult = ORM.GetConnection().Insert(specimen);
            // toast saved trip
            Debug.WriteLine("inserted specimen, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved specimen " + specimen.FieldIdentification);
        }

        public async void btnSetSpecimenGPS_Clicked(object sender, EventArgs e)
        {
            specimenGPS = await CurrentGPS.CurrentLocation();
            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Got GPS");
        }

        public void btnNewSpecimen_Clicked(object sender, EventArgs e)
        {
            specimen = new Specimen();

            fieldID = "";
            occurrenceNotes = "";
            substrate = "";
            lifeStage = "";
            cultivated = false;
            individualCount = "";
            specimenGPS = "";

            entryFieldID.Text = "";
            entryOccurrenceNotes.Text = "";
            entrySubstrate.Text = "";
            pickerLifeStage.SelectedItem = null;
            switchCultivated.IsToggled = false;
            entryIndivCount.Text = "";

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new specimen");
        }
    }
}
