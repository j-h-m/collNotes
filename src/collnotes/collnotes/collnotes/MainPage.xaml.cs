using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;

/*
 * This is collNotes. A replacement, or supplement, field notebook for plant collectors.
 * The most important entity is the User, the second most important is the programmer.
 * As such, it is intented to make navigation throughout as easy and intuitive as possible for the user.
 * Also, in cases where I could have saved a line or two of code with some special formatting, I have opted
 *   to keep the longer format. This is for readability in the future and to hopefully make this code as 
 *   easy as possible for new programmers to read and understand.
 * 
 * Things to understand before moving further: Xamarin Forms Content Pages, Navigation Stack, ORM, LINQ, and Xamarin Plugins aka 'Xamarin Essentials'
 */

namespace collnotes
{
    /// <summary>
    /// Main page.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.MainPage"/> class.
        /// Empty constructor, is only used by the Device Preview feature of Visual Studio.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// NewProject Click event.
        /// Navigates the User to the ProjectPage.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void NewProject_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectPage());
        }

        /// <summary>
        /// Collect Click event.
        /// Asks for input before navigating to Collect Page.
        /// User must choose a Project to Collect under.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void Collect_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<Project> projectList = DataFunctions.GetProjects();

                if (projectList.Count < 1)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Can't start collecting until you've created a project!");
                    return;
                }

                // default 'today' project
                Project todayProject = new Project
                {
                    ProjectName = string.Format("Project-{0}", DateTime.Now.ToString("MM-dd-yyyy")),
                    PrimaryCollector = (AppVariables.CollectorName is null) ? "" : AppVariables.CollectorName,
                    CreatedDate = DateTime.Now
                };

                if (!DataFunctions.CheckExists(todayProject, todayProject.ProjectName))
                {
                    projectList.Add(todayProject);
                    int autoKeyResult = DataFunctions.InsertObject(todayProject);
                }

                string[] projects = (from p in projectList
                            select p.ProjectName).ToArray();

                var action = await DisplayActionSheet("Choose a project", "Cancel", null, projects);

                var projectChosen = (from el in projectList
                                    where el.ProjectName == action
                                    select el).First();

                await Navigation.PushAsync(new CollectingPage(projectChosen));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Can't start collecting until you've created a project!");
            }
        }

        /// <summary>
        /// ExportProject Click event.
        /// Asks the User to select a Project.
        /// Takes User to the ExportProject Page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void ExportProject_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<Project> projectList = DataFunctions.GetProjects();

                if (projectList.Count > 0)
                {
                    await Navigation.PushAsync(new ExportProject());
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No projects to export!");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("No projects to export!");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Settings Click event.
        /// Takes the User to the SettingsPage.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void Settings_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
