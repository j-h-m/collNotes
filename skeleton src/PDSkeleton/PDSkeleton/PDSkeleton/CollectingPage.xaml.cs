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
            // goes under Project

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
                        await Navigation.PushAsync(new SitePage(t));
                    }
                }
            }
            catch (Exception ex)
            {
                // probably no trips
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Trip first!");
                Debug.WriteLine(ex.Message);
            }
        }

        public async void btnAddSpecimen_Clicked(object sender, EventArgs e)
        {
            // popup
            // user chooses existing Site to add Specimen to

            try
            {
                // get all sites for current Project
                List<Trip> tripList = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + project.ProjectName + "'");

                List<Site> allSites = new List<Site>();

                foreach (Trip trip in tripList)
                {
                    List<Site> tripSiteList = ORM.GetConnection().Query<Site>("select * from Site where TripName = '" + trip.TripName + "'");
                    foreach (Site site in tripSiteList)
                    {
                        allSites.Add(site);
                    }
                }

                string[] sites = new string[allSites.Count];
                for (int i = 0; i < sites.Length; i++)
                {
                    sites[i] = allSites[i].SiteName;
                }

                var action = await DisplayActionSheet("Choose a site", "Cancel", null, sites);

                Debug.WriteLine("Site chosen: " + action);

                foreach (Site s in allSites)
                {
                    if (s.SiteName == action)
                    {
                        await Navigation.PushAsync(new SpecimenPage(s));
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Site first!");
                Debug.WriteLine(ex.Message);
            }
        }

        public async void btnInstructions_Clicked(object sender, EventArgs e)
        {
            // use help default for now
            await Navigation.PushAsync(new Help());
        }
	}
}