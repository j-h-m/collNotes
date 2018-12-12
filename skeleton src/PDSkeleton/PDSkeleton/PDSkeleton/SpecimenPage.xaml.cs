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

        private bool editing = false;

        private string specimenGPS = "";
        private string siteName = "";

        // default constructor for xaml preview
        public SpecimenPage() { }

        // constructor takes Site as argument to record Specimen under
        public SpecimenPage(Site site)
        {
            specimen = new Specimen();
            this.site = site;
            siteName = site.SiteName;
            this.Title = siteName + "-" + "s1";
            InitializeComponent();
        }

        // constructor takes Specimen as argument- editing
        public SpecimenPage(Specimen specimen)
        {
            this.specimen = specimen;
            siteName = specimen.SiteName;
            InitializeComponent();

            editing = true;

            entryFieldID.Text = specimen.FieldIdentification;
            entryFieldID.IsEnabled = false;
            entryOccurrenceNotes.Text = specimen.OccurrenceNotes;
            entrySubstrate.Text = specimen.Substrate;
            pickerLifeStage.SelectedItem = specimen.LifeStage;
            switchCultivated.IsToggled = specimen.Cultivated;
            entryIndivCount.Text = specimen.IndividualCount;
            btnNewSpecimen.IsEnabled = false;
        }

        // picker life stage event
        public void pickerLifeStage_SelectedIndexChange(object sender, EventArgs e)
        {
            // picker reset for new Specimen
            if (pickerLifeStage.SelectedItem == null)
            {
                return;
            }
            specimen.LifeStage = pickerLifeStage.SelectedItem.ToString();
            if (specimen.LifeStage.Equals("Other")) 
            {
                lblOtherLifeStage.IsVisible = true;
                entryOtherLifeStage.IsVisible = true;
            }
            else 
            {
                lblOtherLifeStage.IsVisible = false;
                entryOtherLifeStage.IsVisible = false;
            }
        }

        // switch cultivated? event
        public void switchCultivated_Toggled(object sender, EventArgs e)
        {
            specimen.Cultivated = switchCultivated.IsToggled;
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

            if (siteName.Equals("") || entryFieldID.Text.Equals(""))
            {
                // toast need specimen id
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter specimen field ID before taking photo");
                return;
            }

            await TakePhoto.CallCamera(siteName + "-" + entryFieldID.Text);
        }

        public void btnSaveSpecimen_Clicked(object sender, EventArgs e)
        {
            if (editing)
            {
                if (!entrySubstrate.Text.Equals("") && !entryIndivCount.Text.Equals("") && !entryOccurrenceNotes.Text.Equals("") && !(pickerLifeStage.SelectedItem is null) &&
                   !(entrySubstrate.Text is null) && !(entryIndivCount.Text is null) && !(entryOccurrenceNotes.Text is null))
                {
                    specimen.Substrate = entrySubstrate.Text;
                    specimen.IndividualCount = entryIndivCount.Text;
                    specimen.Cultivated = switchCultivated.IsToggled;
                    specimen.OccurrenceNotes = entryOccurrenceNotes.Text;
                    specimen.LifeStage = pickerLifeStage.SelectedItem.ToString();
                    specimen.GPSCoordinates = (specimenGPS.Equals("")) ? specimen.GPSCoordinates : specimenGPS;

                    int updateResult = ORM.GetConnection().Update(specimen, typeof(Specimen));
                    if (updateResult == 1)
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(specimen.FieldIdentification + " save succeeded.");
                        return;
                    }
                    else
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(specimen.FieldIdentification + " save failed");
                        return;
                    }
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info to save Specimen!");
                    return;
                }
            }

            specimen.SiteName = site.SiteName;

            // check to make sure all data is present
            if (entryFieldID.Text is null || entryOccurrenceNotes.Text is null || entrySubstrate.Text is null || entryIndivCount.Text is null || pickerLifeStage.SelectedItem is null ||
               entryFieldID.Text.Equals("") || entryOccurrenceNotes.Text.Equals("") || entrySubstrate.Text.Equals("") || entryIndivCount.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter all information for Specimen!");
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
            int autoKeyResult = ORM.GetConnection().Insert(specimen);
            Debug.WriteLine("inserted specimen, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved specimen " + specimen.FieldIdentification);
        }

        public async void btnSetSpecimenGPS_Clicked(object sender, EventArgs e)
        {
            lblStatusMessage.IsVisible = true;
            lblStatusMessage.TextColor = Color.Orange;
            lblStatusMessage.Text = "Getting Location...";

            pbProgressStatus.IsVisible = true;
            await pbProgressStatus.ProgressTo(1.0, 5, Easing.Linear);

            specimenGPS = await CurrentGPS.CurrentLocation();


            if (specimenGPS.Equals(""))
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

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Specimen");
        }
    }
}
