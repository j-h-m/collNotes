using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;
using collnotes.Plugins;

using System.Linq;

namespace collnotes
{
    /// <summary>
    /// Collecting page.
    /// </summary>
    public partial class CollectingPage : ContentPage
    {
        private Project project;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.CollectingPage"/> class.
        /// </summary>
        public CollectingPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.CollectingPage"/> class.
        /// </summary>
        /// <param name="project">Project.</param>
        public CollectingPage(Project project)
        {
            this.project = project;
            AppVariables.LastProject = project.ProjectName;
            InitializeComponent();
        }

        /// <summary>
        /// AddTrip Click event.
        /// Takes the User to the TripPage.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void AddTrip_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripPage(project));
        }

        /// <summary>
        /// AddSite Click event.
        /// Asks the User to choose a Trip to collect under.
        /// Takes the User to the SitePage.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void AddSite_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = DataFunctions.GetTrips(project.ProjectName);

                if (tripList.Count == 0)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Trip first!");
                    return;
                }

                var trips = (from el in tripList
                           select el.TripName).ToArray();

                var action = await DisplayActionSheet("Choose a Trip", "Cancel", null, trips);

                Debug.WriteLine("Trip chosen: " + action);

                // handle "Cancel" selected
                if (action.Equals("Cancel"))
                {
                    return;
                }

                Trip tripChosen = (from el in tripList
                                 where el.TripName == action
                                 select el).First();

                await Navigation.PushAsync(new SitePage(tripChosen));
            }
            catch (Exception ex)
            {
                // no Trips created, so no Trip database to query
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Trip first!");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// AddSpecimen Click event.
        /// Prompts the User with a Site to collect under.
        /// If the User selects a Site, they are taken to the SpecimenPage.
        /// If the User selects the default new Specimen, it is created and the User stays on the CollectingPage.
        /// If the User selects cancel, nothing happens, and the User stays on the CollectingPage.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void AddSpecimen_Clicked(object sender, EventArgs e)
        {
            try
            {
                // get all sites for current Project
                List<Site> siteList = DataFunctions.GetSitesByProjectName(project.ProjectName);

                var siteNames = (from el in siteList
                                 select el.SiteName).ToList();
                siteNames.Add("Specimen-" + (AppVariables.CollectionCount + 1).ToString());

                var action = await DisplayActionSheet("Choose a Site, or add default Specimen", "Cancel", null, siteNames.ToArray());

                Debug.WriteLine("Action chosen: " + action);

                if (action.Equals("Cancel"))
                {
                    return;
                }

                if (action.Contains("Specimen"))
                {
                    // if trip-today exists, add to it
                    // else add trip-today, add to it
                    Trip trip = new Trip
                    {
                        ProjectName = project.ProjectName,
                        TripName = "Trip-" + DateTime.Now.ToString("MM-dd-yyyy"),
                        CollectionDate = DateTime.Now
                    };
                    if (!DataFunctions.CheckExists(new Trip(), "Trip-" + DateTime.Now.ToString("MM-dd-yyyy")))
                    {
                        DataFunctions.InsertObject(trip);
                    }
                    // if site-today exists, add to it
                    // else add site-today, add to it
                    Site site = new Site
                    {
                        SiteName = "Site-" + DateTime.Now.ToString("MM-dd-yyyy"),
                        TripName = trip.TripName
                    };
                    if (!DataFunctions.CheckExists(site, site.SiteName))
                    {
                        Plugin.Geolocator.Abstractions.Position position = await CurrentGPS.CurrentLocation();
                        if (!(position is null))
                        {
                            site.GPSCoordinates = position.Latitude.ToString() + "," + position.Longitude.ToString() + "," + position.Accuracy.ToString() + "," + position.Altitude.ToString();
                        }
                        DataFunctions.InsertObject(site);
                    }
                    // add this specimen to the specimen database
                    // message user that specimen was added
                    Specimen specimen = new Specimen
                    {
                        SiteName = site.SiteName,
                        SpecimenName = action,
                        SpecimenNumber = AppVariables.CollectionCount,
                        GPSCoordinates = site.GPSCoordinates
                    };

                    DataFunctions.InsertObject(specimen);
                    AppVariables.CollectionCount += 1;

                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(action + " saved!");

                    // anytime we add a specimen we need to write back the CollectionCount
                    AppVarsFile.WriteAppVars();
                }
                else
                {
                    await Navigation.PushAsync(new SpecimenPage(DataFunctions.GetSiteByName(action)));
                }
            }
            catch (Exception ex)
            {
                // no Sites have been saved, no Site table to query against
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Create Site first!");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// EditPage Click event.
        /// Takes the User to the EditPage with the current selected Project to edit.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void EditPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPage(project));
        }
    }
}
