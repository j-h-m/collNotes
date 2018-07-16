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
	public partial class SitePage : ContentPage
	{
        private Trip trip;
        private Site site;
        private string locality = "";
        private string habitat = "";
        private string associatedTaxa = "";
        private string locationNotes = "";
        private Dictionary<string, double> locationInfo;
        private Plugin.Media.Abstractions.MediaFile photo;

        public SitePage (Trip trip)
		{
            InitializeComponent();
            this.trip = trip;
            site = new Site();
		}

        public void Locality_EntryCompleted(object sender, EventArgs e)
        {
            locality = ((Entry)sender).Text;
        }

        public void Habitat_EntryCompleted(object sender, EventArgs e)
        {
            habitat = ((Entry)sender).Text;
        }

        public void AssociatedTaxa_EntryCompleted(object sender, EventArgs e)
        {
            associatedTaxa = ((Entry)sender).Text;
        }

        public void LocationNotes_EntryCompleted(object sender, EventArgs e)
        {
            locationNotes = ((Entry)sender).Text;
        }

        public async void SetSiteGPS_OnClick(object sender, EventArgs e)
        {
            locationInfo = await CurrentGPS.CurrentLocation();
        }        

        public async void SitePhoto_OnClick(object sender, EventArgs e)
        {
            photo = await TakePhoto.CallCamera();
        }

        public void SaveSite_OnClick(object sender, EventArgs e)
        {
            site.AssociatedTaxa = associatedTaxa;
            site.Habitat = habitat;
            site.Locality = locality;
            site.LocationNotes = locationNotes;
            site.SiteGPS = locationInfo;
            site.photo = this.photo;
            site.Specimen = new List<Specimen>();
            trip.Sites.Add(site);
        }
        
        public async void NewSpecimen_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SpecimenPage(site));
        }
    }
}