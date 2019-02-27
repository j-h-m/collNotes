using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace collnotes
{
    public partial class CollectingPage : ContentPage
    {
        private Project project;

        public CollectingPage()
        {
            InitializeComponent();
        }

        public CollectingPage(Project project)
        {
            this.project = project;
            AppVariables.LastProject = project.ProjectName;
            InitializeComponent();
        }

        public async void btnAddTrip_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripPage(project));
        }

        public async void btnAddSite_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = ORM.GetTrips(project.ProjectName);

                if (tripList.Count == 0) {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Trip first!");
                    return;
                }

                string[] trips = new string[tripList.Count];
                for (int i = 0; i < trips.Length; i++) {
                    trips[i] = tripList[i].TripName;
                }

                var action = await DisplayActionSheet("Choose a Trip", "Cancel", null, trips);

                Debug.WriteLine("Trip chosen: " + action);

                foreach (Trip t in tripList) {
                    if (t.TripName == action) {
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

        public async void btnAddSpecimen_Clicked(object sender, EventArgs e)
        {
            try
            {
                // get all sites for current Project
                List<Trip> tripList = ORM.GetTrips(project.ProjectName);

                List<Site> allSites = new List<Site>();

                foreach (Trip trip in tripList) {
                    List<Site> tripSiteList = ORM.GetSites(trip.TripName);
                    foreach (Site site in tripSiteList) {
                        allSites.Add(site);
                    }
                }

                string[] sites = new string[allSites.Count + 1];
                for (int i = 0; i < sites.Length - 1; i++) {
                    sites[i] = allSites[i].SiteName;
                }

                sites[allSites.Count] = "Specimen" + (AppVariables.CollectionCount + 1).ToString();
                AppVariables.CollectionCount += 1;

                var action = await DisplayActionSheet("Choose a Site, or add default Specimen", "Cancel", null, sites);

                Debug.WriteLine("Action chosen: " + action);

                if (action.Contains("Specimen")) {
                    // if trip-today exists, add to it
                    // else add trip-today, add to it
                    Trip trip = new Trip
                    {
                        ProjectName = project.ProjectName,
                        TripName = "Trip-" + DateTime.Now.ToString("MM-dd-yyyy"),
                        CollectionDate = DateTime.Now
                    };
                    if (!ORM.CheckExists(trip)) {
                        ORM.InsertObject(trip);
                    }
                    // if site-today exists, add to it
                    // else add site-today, add to it
                    Site site = new Site
                    {
                        SiteName = "Site-" + DateTime.Now.ToString("MM-dd-yyyy"),
                        TripName = trip.TripName,
                        GPSCoordinates = await CurrentGPS.CurrentLocation()
                    };
                    if (!ORM.CheckExists(site)) {
                        ORM.InsertObject(site);
                    }
                    // add this specimen to the specimen database
                    // message user that specimen was added
                    Specimen specimen = new Specimen();
                    specimen.SiteName = site.SiteName;
                    specimen.SpecimenName = action;
                    specimen.SpecimenNumber = AppVariables.CollectionCount;
                    specimen.GPSCoordinates = await CurrentGPS.CurrentLocation();

                    ORM.InsertObject(specimen);

                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(action + " saved!");
                }
                else {
                    await Navigation.PushAsync(new SpecimenPage(ORM.GetSiteByName(action)));
                }
            }
            catch (Exception ex)
            {
                // no Sites have been saved, no Site table to query against
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Site first!");
                Debug.WriteLine(ex.Message);
            }
        }

        public async void btnEditPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPage(project));
        }
    }
}
