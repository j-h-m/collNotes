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
		public SitePage ()
		{
			InitializeComponent ();
		}

        public void Locality_EntryCompleted(object sender, EventArgs e)
        {
            
        }

        public void Habitat_EntryCompleted(object sender, EventArgs e)
        {

        }

        public void AssociatedTaxa_EntryCompleted(object sender, EventArgs e)
        {

        }

        public void LocationNotes_EntryCompleted(object sender, EventArgs e)
        {

        }

        public void SetSiteGPS_OnClick(object sender, EventArgs e)
        {

        }        

        public void SitePhoto_OnClick(object sender, EventArgs e)
        {

        }

        public void SaveSite_OnClick(object sender, EventArgs e)
        {

        }
        
        public async void NewSpecimen_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SpecimenPage());
        }
    }
}