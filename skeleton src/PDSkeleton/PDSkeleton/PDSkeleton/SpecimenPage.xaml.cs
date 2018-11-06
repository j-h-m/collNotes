using System;
using System.Collections.Generic;
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
            specimen.FieldIdentification = entryFieldID.Text;
        }

        // occurrence notes text entry event
        public void entryOccurrenceNotes_Completed(object sender, EventArgs e)
        {
            specimen.OccurrenceNotes = entryOccurrenceNotes.Text;
        }

        // substrate text entry event
        public void entrySubstrate_Completed(object sender, EventArgs e)
        {
            specimen.Substrate = entrySubstrate.Text;
        }

        // picker life stage event
        public void pickerLifeStage_SelectedIndexChange(object sender, EventArgs e)
        {
            specimen.LifeStage = pickerLifeStage.SelectedItem.ToString();
            if (specimen.LifeStage.Equals("Other")) {
                lblOtherLifeStage.IsVisible = true;
                entryOtherLifeStage.IsVisible = true;
            }
            else {
                lblOtherLifeStage.IsVisible = false;
                entryOtherLifeStage.IsVisible = false;
            }
        }

        // switch cultivated? event
        public void switchCultivated_Toggled(object sender, EventArgs e)
        {
            specimen.Cultivated = switchCultivated.IsToggled;
        }

        // individual count text entry event
        public void entryIndivCount_Completed(object sender, EventArgs e)
        {
            specimen.IndividualCount = entryIndivCount.Text;
        }

        // specimen photo button event
        public async void btnSetSpecimenPhoto_Clicked(object sender, EventArgs e)
        {
            // get specimen name first
            if (entryFieldID.Text == null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need a specimen field ID before taking photo");
                return;
            }

            if (site.SiteName.Equals("") || entryFieldID.Text.Equals(""))
            {
                // toast need specimen id
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter specimen field ID before taking photo");
                return;
            }

            await TakePhoto.CallCamera(site.SiteName + "-" + entryFieldID.Text);
        }

        public void btnSaveSpecimen_Clicked(object sender, EventArgs e)
        {
            specimen.SiteName = site.SiteName;

            // check to make sure all data is present
            if (entryFieldID.Text is null || entryOccurrenceNotes.Text is null || entrySubstrate.Text is null || entryIndivCount.Text is null || pickerLifeStage.SelectedItem is null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info to save Specimen");
                return;
            }

            specimen.GPSCoordinates = specimenGPS;
            specimen.FieldIdentification = entryFieldID.Text;
            specimen.OccurrenceNotes = entryOccurrenceNotes.Text;
            specimen.Substrate = entrySubstrate.Text;
            specimen.IndividualCount = entryIndivCount.Text;


            if (entryOtherLifeStage.IsVisible && !entryOtherLifeStage.Text.Equals("")) {
                specimen.LifeStage = entryOtherLifeStage.Text;
            }
            else if (entryOtherLifeStage.IsVisible){
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Fill in \"Other\" Life Stage!");
                return;
            }
            else {
                specimen.LifeStage = pickerLifeStage.SelectedItem.ToString();
            }

            specimen.Cultivated = switchCultivated.IsToggled;

            // save Specimen to database
            ORM.GetConnection().CreateTable<Specimen>();
            int autoKeyResult = ORM.GetConnection().Insert(specimen);
            Debug.WriteLine("inserted specimen, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved specimen " + specimen.FieldIdentification);
        }

        public async void btnSetSpecimenGPS_Clicked(object sender, EventArgs e)
        {
            specimenGPS = await CurrentGPS.CurrentLocation();
            lblStatusMessage.IsVisible = true;
            lblStatusMessage.TextColor = Color.Orange;
            lblStatusMessage.Text = "Getting Location...";

            if (specimenGPS.Equals(""))
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
                lblStatusMessage.Text = "Location: " + specimenGPS;
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Location: " + specimenGPS);
            }
        }

        public void btnNewSpecimen_Clicked(object sender, EventArgs e)
        {
            specimen = new Specimen();

            specimenGPS = "";

            entryFieldID.Text = "";
            entryOccurrenceNotes.Text = "";
            entrySubstrate.Text = "";
            pickerLifeStage.SelectedItem = null;
            switchCultivated.IsToggled = false;
            entryIndivCount.Text = "";

            lblOtherLifeStage.IsVisible = false;
            entryOtherLifeStage.IsVisible = false;
            entryOtherLifeStage.Text = "";

            lblStatusMessage.IsVisible = false;
            lblStatusMessage.Text = "";

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new specimen");
        }
    }
}
