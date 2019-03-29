using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;

namespace collnotes
{
    public partial class EditPage : ContentPage
    {
        private Project project;

        public EditPage()
        {
            InitializeComponent();
        }

        public EditPage(Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        public async void btnEditProject_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectPage(project));
        }

        public async void btnEditTrip_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = DataFunctions.GetTrips(project.ProjectName);

                if (tripList.Count > 0)
                {
                    string[] trips = new string[tripList.Count];
                    for (int i = 0; i < trips.Length; i++)
                    {
                        trips[i] = tripList[i].TripName;
                    }

                    var action = await DisplayActionSheet("Choose a Trip", "Cancel", null, trips);

                    foreach (Trip t in tripList)
                    {
                        if (t.TripName == action)
                        {
                            await Navigation.PushAsync(new TripPage(t));
                            break;
                        }
                    }
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Trips recorded for current Project");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Are there any Trips for this Project?");
            }
        }

        public async void btnEditSite_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = DataFunctions.GetTrips(project.ProjectName);

                List<Site> siteList = new List<Site>();

                foreach (Trip trip in tripList)
                {
                    siteList.AddRange(DataFunctions.GetSites(trip.TripName));
                }

                if (siteList.Count > 0)
                {
                    string[] sites = new string[siteList.Count];
                    for (int i = 0; i < sites.Length; i++)
                    {
                        sites[i] = siteList[i].SiteName;
                    }

                    var action = await DisplayActionSheet("Choose a Site", "Cancel", null, sites);

                    foreach (Site s in siteList)
                    {
                        if (s.SiteName == action)
                        {
                            await Navigation.PushAsync(new SitePage(s));
                            break;
                        }
                    }
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Sites recorded for current Project");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Are there any Sites for this Project?");
            }
        }

        public async void btnEditSpecimen_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = DataFunctions.GetTrips(project.ProjectName);

                List<Site> siteList = new List<Site>();

                foreach (Trip trip in tripList)
                {
                    siteList.AddRange(DataFunctions.GetSites(trip.TripName));
                }

                List<Specimen> specimenList = new List<Specimen>();

                foreach (Site site in siteList)
                {
                    specimenList.AddRange(DataFunctions.GetSpecimen(site.SiteName));
                }

                if (specimenList.Count > 0)
                {
                    string[] specimens = new string[specimenList.Count];
                    for (int i = 0; i < specimens.Length; i++)
                    {
                        specimens[i] = specimenList[i].SpecimenName;
                    }

                    var action = await DisplayActionSheet("Choose a Specimen", "Cancel", null, specimens);

                    foreach (Specimen s in specimenList)
                    {
                        if (s.SpecimenName.Equals(action))
                        {
                            await Navigation.PushAsync(new SpecimenPage(s));
                            break;
                        }
                    }
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Specimen recorded for current Project");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Are there any Specimen for this Project?");
            }
        }
    }
}
