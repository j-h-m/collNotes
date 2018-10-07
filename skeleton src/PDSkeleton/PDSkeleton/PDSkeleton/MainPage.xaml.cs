using System;
using System.Diagnostics;
using System.Collections.Generic;
using Xamarin.Forms;

/*
 * Main Menu Page
 *  - it is the main navigation page which is launched on App load
 */ 

namespace PDSkeleton
{
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        // NewProject button event
        //  - loads Project page
        public async void NewProject_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectPage());
        }

        // Collect button event
        //  - loads a list of Projects for the user to choose to collect under
        //  - upon user choosing, loads the Collecting Page
        public async void Collect_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<Project> projectList = ORM.GetConnection().Query<Project>("select * from Project");

                string[] projects = new string[projectList.Count];
                for (int i = 0; i < projects.Length; i++)
                {
                    projects[i] = projectList[i].ProjectName;
                }

                var action = await DisplayActionSheet("Choose a project", "Cancel", null, projects);

                foreach (Project p in projectList)
                {
                    if (p.ProjectName == action)
                    {
                        await Navigation.PushAsync(new CollectingPage(p));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Can't start collecting until you've created a project!");
            }
        }

        // Export Project button event
        //  - checks the local database for Projects before navigation
        //  - loads the ExportProject Page
        public async void ExportProject_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<Project> projectList = ORM.GetConnection().Query<Project>("select * from Project");

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

        // Help button event
        //  - loads the Help Page
        public async void Help_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Help());
        }
    }
}