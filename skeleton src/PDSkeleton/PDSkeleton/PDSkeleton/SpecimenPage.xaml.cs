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
	public partial class SpecimenPage : ContentPage
	{
        private Site site;
        private Specimen specimen;
        private Dictionary<string, double> specimenGPS = new Dictionary<string, double>();
        private string fieldID = "";
        private string occurrenceNotes = "";
        private string substrate = "";
        private string lifeStage = "";
        private string additionalInfo = "";
        private int individualCount = -1; // -1 indicates it hasn't been set
        private bool cultivated = false;
        private int myRecordNo = 0;
        public Plugin.Media.Abstractions.MediaFile photo;

        public SpecimenPage()
        {
            InitializeComponent();
        }

        public SpecimenPage(Site site)
        {
            InitializeComponent();
            this.site = site;
            specimen = new Specimen();
        }

        public async void SetSpecimenGPS_OnClick(object sender, EventArgs e)
        {
            // get the current location
            //specimenGPS = await CurrentGPS.CurrentLocation();
        }

        public void FieldID_EntryCompleted(object sender, EventArgs e)
        {
            fieldID = ((Entry)sender).Text;
        }

        public void OccurrenceNotes_EntryCompleted(object sender, EventArgs e)
        {
            occurrenceNotes = ((Entry)sender).Text;
        }

        public void Substrate_EntryCompleted(object sender, EventArgs e)
        {
            substrate = ((Entry)sender).Text;
        }

        public void Picker_SelectedIndexChanged(object sender, EventArgs e) // life stage
        {
            lifeStage = ((Picker)sender).SelectedItem.ToString();
        }

        public void Cultivated_SwitchToggled(object sender, EventArgs e)
        {
            cultivated = ((Switch)sender).IsToggled; // toggled implies true?
        }

        public void IndivCount_EntryCompleted(object sender, EventArgs e)
        {
            int.TryParse(((Entry)sender).Text, out individualCount);
        }

        public async void SpecimenPhoto_OnClick(object sender, EventArgs e)
        {
            photo = await TakePhoto.CallCamera();
        }

        public void SaveSpecimen_OnClick(object sender, EventArgs e)
        {
            //specimen.AdditionalInfo = additionalInfo;
            //specimen.Cultivated = cultivated;
            //specimen.FieldID = fieldID;
            //specimen.IndividualCount = individualCount;
            //specimen.LifeStage = lifeStage;
            //specimen.OccurrenceNotes = occurrenceNotes;
            //specimen.Substrate = substrate;
            //specimen.photo = this.photo;
            //site.Specimen.Add(specimen);

            // write to sqlite database


            // pass recordno to lower heirarchy object


        }
    }
}