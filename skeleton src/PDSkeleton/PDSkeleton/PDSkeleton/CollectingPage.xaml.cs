using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

/*
 * Collecting Page 
 *  - gets Project through navigation from MainPage
 *  - Menu to create Trips, Sites, and Specimen under the selected Project
 */

namespace PDSkeleton
{
    public partial class CollectingPage : ContentPage
	{
        // fields
        private Project project;

        // constructor accepts selected Project from MainPage
        public CollectingPage (Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        // AddTrip button
        //  - loads the Trip Page
        //  - passes selected project from MainPage to TripPage through TripPage's constructor
        public async void btnAddTrip_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripPage(project));
        }

        // AddSite button
        //  - prompts user with list of Trips to add a Site to
        //  - selected Trip is passed to the SitePage through SitePage's constructor
        public async void btnAddSite_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + project.ProjectName + "'");

                if (tripList.Count == 0)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Trip first!");
                    return;
                }

                string[] trips = new string[tripList.Count];
                for (int i = 0; i < trips.Length; i++)
                {
                    trips[i] = tripList[i].TripName;
                }

                var action = await DisplayActionSheet("Choose a Trip", "Cancel", null, trips);

                Debug.WriteLine("Trip chosen: " + action);

                foreach (Trip t in tripList)
                {
                    if (t.TripName == action)
                    {
                        await Navigation.PushAsync(new SitePage(t));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // no Trips created, so no Trip database to query
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Trip first!");
                Debug.WriteLine(ex.Message);
            }
        }

        // Add Specimen button
        //  - prompts user with a Site to add Specimen to
        //  - upon selecting a Site, the Site is passed to the SpecimenPage through SpecimenPage's constructor
        public async void btnAddSpecimen_Clicked(object sender, EventArgs e)
        {
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

                if (allSites.Count == 0)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Site first!");
                }

                string[] sites = new string[allSites.Count];
                for (int i = 0; i < sites.Length; i++)
                {
                    sites[i] = allSites[i].SiteName;
                }

                var action = await DisplayActionSheet("Choose a Site", "Cancel", null, sites);

                Debug.WriteLine("Site chosen: " + action);

                foreach (Site s in allSites)
                {
                    if (s.SiteName == action)
                    {
                        await Navigation.PushAsync(new SpecimenPage(s));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // no Sites have been saved, no Site table to query against
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Site first!");
                Debug.WriteLine(ex.Message);
            }
        }

        // Navigate to Edit Page
        public async void btnEditPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPage(project));
        }
	}
}