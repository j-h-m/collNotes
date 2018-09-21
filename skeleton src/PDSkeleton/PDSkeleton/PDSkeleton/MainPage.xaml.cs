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
            // use overload constructor for CollectingPage with project

            SQLite.SQLiteConnection connection = ORM.GetConnection();
            List<Project> projectList = connection.Query<Project>("select * from Project");

            string[] projects = new string[projectList.Count];
            for (int i = 0; i < projects.Length; i++)
            {
                projects[i] = projectList[i].ProjectName;
            }

            var action = await DisplayActionSheet("Choose a project!", "Cancel", null, projects);

            Debug.WriteLine("Project chosen: " + action);

            foreach (var item in projectList)
            {
                if (item.ProjectName == action)
                {
                    await Navigation.PushAsync(new CollectingPage(item));
                }
            }
        }
        
        public async void Settings_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }

        public async void Help_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Help());
        }
    }
}