using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;
using System.Linq;

namespace collnotes
{
    /// <summary>
    /// Edit page.
    /// </summary>
    public partial class EditPage : ContentPage
    {
        private Project project;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.EditPage"/> class.
        /// </summary>
        public EditPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.EditPage"/> class.
        /// </summary>
        /// <param name="project">Project.</param>
        public EditPage(Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        /// <summary>
        /// Buttons the edit project clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void EditProject_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectPage(project));
        }

        /// <summary>
        /// Buttons the edit trip clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void EditTrip_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = DataFunctions.GetTrips(project.ProjectName);

                if (tripList.Count > 0)
                {
                    string[] trips = (from el in tripList
                                     select el.TripName).ToArray();

                    var action = await DisplayActionSheet("Choose a Trip", "Cancel", null, trips);

                    if (action.Equals("Cancel"))
                    {
                        return;
                    }

                    var tripChosen = (from el in tripList
                                      where el.TripName == action
                                      select el).First();

                    await Navigation.PushAsync(new TripPage(tripChosen));
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Trips recorded for current Project");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Trips recorded for current Project");
            }
        }

        /// <summary>
        /// Buttons the edit site clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void EditSite_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Site> siteList = DataFunctions.GetSitesByProjectName(project.ProjectName);

                if (siteList.Count > 0)
                {
                    string[] sites = (from el in siteList
                    select el.SiteName).ToArray();

                    var action = await DisplayActionSheet("Choose a Site", "Cancel", null, sites);

                    if (action.Equals("Cancel"))
                    {
                        return;
                    }

                    var siteChosen = (from el in siteList
                                      where el.SiteName == action
                                      select el).First();

                    await Navigation.PushAsync(new SitePage(siteChosen));
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Sites recorded for current Project");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Sites recorded for current Project");
            }
        }

        /// <summary>
        /// Buttons the edit specimen clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void EditSpecimen_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Specimen> specimenList = DataFunctions.GetSpecimenByProjectName(project.ProjectName);

                if (specimenList.Count > 0)
                {
                    string[] specimens = (from el in specimenList
                                          select el.SpecimenName).ToArray();

                    var action = await DisplayActionSheet("Choose a Specimen", "Cancel", null, specimens);

                    if (action.Equals("Cancel"))
                    {
                        return;
                    }

                    var specimenChosen = (from el in specimenList
                                          where el.SpecimenName == action
                                          select el).First();

                    await Navigation.PushAsync(new SpecimenPage(specimenChosen));
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Specimen recorded for current Project");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("No Specimen recorded for current Project");
            }
        }
    }
}
