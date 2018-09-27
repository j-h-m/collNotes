using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

/*
 * Collecting Page - gets Project through navigation from MainPage
 * Menu to create Trips, Sites, and Specimen under the selected Project
 * 
 */

namespace PDSkeleton
{
    public partial class CollectingPage : ContentPage
	{
        private Project project;
        private Trip selectedTrip = null;

        public CollectingPage (Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        public async void btnAddTrip_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripPage(project));
        }

        public async void btnAddSite_Clicked(object sender, EventArgs e)
        {
            // popup
            // user chooses existing Trip to add Site to

            try
            {
                List<Trip> tripList = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + project.ProjectName + "'");

                string[] trips = new string[tripList.Count];
                for (int i = 0; i < trips.Length; i++)
                {
                    trips[i] = tripList[i].TripName;
                }

                var action = await DisplayActionSheet("Choose a trip", "Cancel", null, trips);

                Debug.WriteLine("Trip chosen: " + action);

                foreach (Trip t in tripList)
                {
                    if (t.TripName == action)
                    {
                        selectedTrip = t;
                        await Navigation.PushAsync(new SitePage(t));
                    }
                }
            }
            catch (Exception ex)
            {
                // probably no trips
                // tell user to create trip first

                Debug.WriteLine(ex.Message);
            }
        }

        public async void btnAddSpecimen_Clicked(object sender, EventArgs e)
        {
            // popup
            // user chooses existing Site to add Specimen to

            if (selectedTrip == null)
            {
                // user should create a Site first
            }

            try
            {
                List<Site> siteList = ORM.GetConnection().Query<Site>("select * from Site where TripName = '" + selectedTrip.TripName + "'");

                string[] sites = new string[siteList.Count];
                for (int i = 0; i < sites.Length; i++)
                {
                    sites[i] = siteList[i].SiteName;
                }

                var action = await DisplayActionSheet("Choose a site", "Cancel", null, sites);

                Debug.WriteLine("Trip chosen: " + action);

                foreach (Site s in siteList)
                {
                    if (s.SiteName == action)
                    {
                        await Navigation.PushAsync(new SpecimenPage(s));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void btnInstructions_Clicked(object sender, EventArgs e)
        {
            
        }
	}
}