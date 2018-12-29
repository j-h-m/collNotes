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
                List<Trip> tripList = ORM.GetTrips(project.ProjectName);

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
                List<Trip> tripList = ORM.GetTrips(project.ProjectName);

                List<Site> allSites = new List<Site>();

                foreach (Trip trip in tripList)
                {
                    List<Site> tripSiteList = ORM.GetSites(trip.TripName);
                    foreach (Site site in tripSiteList)
                    {
                        allSites.Add(site);
                    }
                }

                /*
                 * add specimen with no Site selected
                 *  - how do i tie this to a project
                 * 
                 */

                //if (allSites.Count == 0)
                //{
                //    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Site first!");
                //    return;
                //}

                string[] sites = new string[allSites.Count + 1];
                for (int i = 0; i < sites.Length - 1; i++)
                {
                    sites[i] = allSites[i].SiteName;
                }

                sites[allSites.Count] = "Specimen" + (AppVariables.CollectionCount + 1).ToString();
                AppVariables.CollectionCount += 1;

                var action = await DisplayActionSheet("Choose a Site", "Cancel", null, sites);

                Debug.WriteLine("Site chosen: " + action);

                foreach (var item in sites)
                {
                    if (item.Contains("Specimen"))
                    {
                        // if trip-today exists, add to it
                        // else add trip-today, add to it
                        Trip trip = new Trip();
                        trip.ProjectName = project.ProjectName;
                        trip.TripName = "Trip-" + DateTime.Now.ToString("MM-dd-yyyy");
                        trip.CollectionDate = DateTime.Now;
                        // if site-today exists, add to it
                        // else add site-today, add to it
                        // ***use data in settings for info
                    }
                    else
                    {
                        await Navigation.PushAsync(new SpecimenPage(ORM.GetSiteByName(item)));
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