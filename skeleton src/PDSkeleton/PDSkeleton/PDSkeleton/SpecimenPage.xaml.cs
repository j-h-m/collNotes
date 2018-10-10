using System;
using System.Diagnostics;
using Xamarin.Forms;

/*
 * Specimen Page
 *  - collect Specimen under a Site
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

        // constructor takes Site as argument to record Specimen under
        public SpecimenPage(Site site)
        {
            specimen = new Specimen();
            this.site = site;
            InitializeComponent();
        }

        // field id text entry event
        public void entryFieldID_Completed(object sender, EventArgs e)
        {
            fieldID = entryFieldID.Text;
        }

        // occurrence notes text entry event
        public void entryOccurrenceNotes_Completed(object sender, EventArgs e)
        {
            occurrenceNotes = entryOccurrenceNotes.Text;
        }

        // substrate text entry event
        public void entrySubstrate_Completed(object sender, EventArgs e)
        {
            substrate = entrySubstrate.Text;
        }

        // picker life stage event
        public void pickerLifeStage_SelectedIndexChange(object sender, EventArgs e)
        {
            lifeStage = pickerLifeStage.SelectedItem.ToString();
        }

        // switch cultivated? event
        public void switchCultivated_Toggled(object sender, EventArgs e)
        {
            cultivated = switchCultivated.IsToggled;
        }

        // individual count text entry event
        public void entryIndivCount_Completed(object sender, EventArgs e)
        {
            individualCount = entryIndivCount.Text;
        }

        // specimen photo button event
        public async void btnSetSpecimenPhoto_Clicked(object sender, EventArgs e)
        {
            // get specimen name first
            if (site.SiteName.Equals("") || entryFieldID.Text.Equals(""))
            {
                // toast need specimen id
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must entry specimen field ID before taking photo");
                return;
            }
            await TakePhoto.CallCamera(site.SiteName + "-" + specimen.FieldIdentification);
        }

        public void btnSaveSpecimen_Clicked(object sender, EventArgs e)
        {
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
            if (specimenGPS.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Failed to get GPS location. Is Location enabled?");
            }
            else
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Location: " + specimenGPS);
            }
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
