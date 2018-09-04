using System;
using System.Diagnostics;
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
            // overload constructor with project
            //string[] projects = new string[SharedLists.Projects.Count];
            //for (int i = 0; i < projects.Length; i++)
            //{
            //    projects[i] = SharedLists.Projects[i].ProjectName;
            //}
            //var action = await DisplayActionSheet("Choose a project!", "Cancel", null, projects);
            //Debug.WriteLine("Project chosen: " + action);
            //foreach (var item in SharedLists.Projects)
            //{
            //    if (item.ProjectName == action)
            //    {
            //await Navigation.PushModalAsync(new CollectingPage(item));
            //    }
            //}
            await Navigation.PushAsync(new CollectingPage());
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
