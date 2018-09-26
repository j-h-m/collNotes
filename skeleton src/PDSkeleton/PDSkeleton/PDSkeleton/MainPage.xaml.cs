using System;
using System.Diagnostics;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PDSkeleton
{
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        public async void NewProject_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectPage());
        }

        public async void Collect_OnClick(object sender, EventArgs e)
        {
            // popup
            // user chooses existing project to collect under

            try
            {
                List<Project> projectList = ORM.GetConnection().Query<Project>("select * from Project");

                string[] projects = new string[projectList.Count];
                for (int i = 0; i < projects.Length; i++)
                {
                    projects[i] = projectList[i].ProjectName;
                }

                var action = await DisplayActionSheet("Choose a project", "Cancel", null, projects);

                Debug.WriteLine("Project chosen: " + action);

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
                // alert user they can't start collecting until they've chosen a project
            }
        }
        
        public async void ExportProject_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExportProject());
        }

        public async void Help_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Help());
        }
    }
}