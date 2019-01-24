using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * Project Page
 *  - users create new Projects to collect under on this Page
 */

namespace PDSkeleton
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjectPage : ContentPage
	{
        private Project project;

        private bool editing = false;

        // constructor for collecting
		public ProjectPage()
		{
            project = new Project();
			InitializeComponent();
		}

        // constructor for editing
        public ProjectPage(Project project)
        {
            this.project = project;
            editing = true;
            InitializeComponent();

            entryProjectName.Text = project.ProjectName;
            entryPrimaryCollectorProject.Text = project.PrimaryCollector;
            dpCreatedDate.Date = project.CreatedDate;
            entryProjectName.IsEnabled = false;
            btnNewProject.IsEnabled = false;
        }

        // date picker created date event
        private void dpCreatedDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            project.CreatedDate = dpCreatedDate.Date;
        }

        // SaveProject button event
        //  - checks for required data on click
        //  - writes Project to the local database
        private async void btnSaveProject_Clicked(object sender, EventArgs e)
        {
            if (editing)
            {
                if (!(entryPrimaryCollectorProject.Text is null) || !entryPrimaryCollectorProject.Text.Equals(""))
                {
                    project.PrimaryCollector = entryPrimaryCollectorProject.Text;
                    project.CreatedDate = dpCreatedDate.Date;

                    int updateResult = ORM.GetConnection().Update(project, typeof(Project));
                    if (updateResult == 1)
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(project.ProjectName + " update succeeded.");
                        return;
                    }
                    else
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(project.ProjectName + " update failed.");
                        return;
                    }
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Update requires some info to be changed.");
                    return;
                }
            }

            // check to make sure name is present
            if (entryProjectName.Text is null || entryProjectName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Unique name required to identify Project");
                return;
            }

            project.ProjectName = entryProjectName.Text;
            // check for default collector name and use if primary collector wasn't given
            string defaultCollectorName = (AppVariables.CollectorName is null) ? "" : AppVariables.CollectorName;
            project.PrimaryCollector = (entryPrimaryCollectorProject.Text.Equals("") || entryPrimaryCollectorProject.Text is null) ? defaultCollectorName : entryPrimaryCollectorProject.Text;
            project.CreatedDate = dpCreatedDate.Date;

            // check for duplicate names first
            if (ORM.CheckExists(project))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert($"'{project}' already exists. Enter a Unique name for a new.");
                return;
            }

            // save project to database
            int autoKeyResult = ORM.GetConnection().Insert(project);
            Debug.WriteLine("inserted project, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Project " + project.ProjectName + " saved");

            // automatically navigate to the collecting page after saving the project
            await Navigation.PushAsync(new CollectingPage(project));
        }

        // NewProject button event
        // - resets project and creates new Project object
        private void btnNewProject_Clicked(object sender, EventArgs e)
        {
            project = new Project();

            entryProjectName.Text = "";
            entryPrimaryCollectorProject.Text = "";

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Project");
        }
    }
}