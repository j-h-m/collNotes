using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace collnotes
{
    public partial class SpecimenPage : ContentPage
    {
        private Site site;
        private Specimen specimen;
        private bool editing = false;

        public SpecimenPage()
        {
            InitializeComponent();
        }

        public SpecimenPage(Site site)
        {
            specimen = new Specimen();
            this.site = site;
            InitializeComponent();
        }

        public SpecimenPage(Specimen specimen)
        {
            this.specimen = specimen;
            Site site = ORM.GetSiteByName(specimen.SiteName);
            InitializeComponent();

            editing = true;

            Title = site.RecordNo + "-" + specimen.SpecimenNumber;
            entryFieldID.Text = specimen.FieldIdentification;
            entryFieldID.IsEnabled = false;
            entryOccurrenceNotes.Text = specimen.OccurrenceNotes;
            entrySubstrate.Text = specimen.Substrate;
            pickerLifeStage.SelectedItem = specimen.LifeStage;
            switchCultivated.IsToggled = specimen.Cultivated;
            entryIndivCount.Text = specimen.IndividualCount;
            btnNewSpecimen.IsEnabled = false;
        }

        private void LoadDefaults()
        {
            Title = "(" + site.SiteName + ")" + site.RecordNo.ToString() + "-" + (AppVariables.CollectionCount + 1).ToString();
        }

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

        public void switchCultivated_Toggled(object sender, EventArgs e)
        {
            specimen.Cultivated = switchCultivated.IsToggled;
        }

        public async void btnSetSpecimenPhoto_Clicked(object sender, EventArgs e)
        {
            await TakePhoto.CallCamera(site.RecordNo.ToString() + "-" + specimen.SpecimenNumber.ToString());
        }

        public void btnSaveSpecimen_Clicked(object sender, EventArgs e)
        {
            if (editing)
            {
                specimen.Substrate = entrySubstrate.Text;
                specimen.IndividualCount = entryIndivCount.Text;
                specimen.Cultivated = switchCultivated.IsToggled;
                specimen.OccurrenceNotes = entryOccurrenceNotes.Text;
                specimen.LifeStage = pickerLifeStage.SelectedItem.ToString();
                specimen.GPSCoordinates = site.GPSCoordinates;

                int updateResult = ORM.GetConnection().Update(specimen, typeof(Specimen));
                if (updateResult == 1)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(specimen.SpecimenName + " save succeeded.");
                    return;
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(specimen.SpecimenName + " save failed");
                    return;
                }
            }

            specimen.SiteName = site.SiteName;
            specimen.GPSCoordinates = site.GPSCoordinates;
            specimen.OccurrenceNotes = entryOccurrenceNotes.Text is null ? "" : entryOccurrenceNotes.Text;
            specimen.Substrate = entrySubstrate.Text is null ? "" : entrySubstrate.Text;
            specimen.IndividualCount = entryIndivCount.Text is null ? "" : entryIndivCount.Text;
            specimen.Cultivated = switchCultivated.IsToggled;

            AppVariables.CollectionCount = AppVariables.CollectionCount > 0 ? AppVariables.CollectionCount : 0;

            specimen.SpecimenNumber = AppVariables.CollectionCount + 1;

            specimen.FieldIdentification = entryFieldID.Text is null ? "" : entryFieldID.Text;

            specimen.SpecimenName = "Specimen-" + specimen.SpecimenNumber;

            if (entryOtherLifeStage.IsVisible && !entryOtherLifeStage.Text.Equals(""))
            {
                specimen.LifeStage = entryOtherLifeStage.Text;
            }
            else if (entryOtherLifeStage.IsVisible)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Fill in \"Other\" Life Stage!");
                return;
            }
            else
            {
                specimen.LifeStage = pickerLifeStage.SelectedItem is null ? "" : pickerLifeStage.SelectedItem.ToString();
            }

            // save Specimen to database
            int autoKeyResult = ORM.GetConnection().Insert(specimen);
            Debug.WriteLine("inserted specimen, recordno is: " + autoKeyResult.ToString());

            // update CollectionCount
            AppVariables.CollectionCount = specimen.SpecimenNumber;

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved specimen " + specimen.SpecimenName);
        }

        public void btnNewSpecimen_Clicked(object sender, EventArgs e)
        {
            specimen = new Specimen();

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

            specimen.SiteName = site.SiteName;

            LoadDefaults();

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Specimen");
        }
    }
}
