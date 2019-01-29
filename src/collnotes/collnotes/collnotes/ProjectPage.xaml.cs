using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace collnotes
{
    public partial class ProjectPage : ContentPage
    {
        private Project project;
        private bool editing = false;
        private bool dateChanged = false;

        public ProjectPage()
        {
            project = new Project();
            InitializeComponent();
            LoadDefaults();
        }

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

        private void LoadDefaults()
        {
            entryPrimaryCollectorProject.Text = (AppVariables.CollectorName is null) ? "" : AppVariables.CollectorName;
            entryProjectName.Text = "Project-" + (ORM.GetAllProjectsCount() + 1).ToString();
            dpCreatedDate.Date = DateTime.Today;
        }

        private void dpCreatedDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            project.CreatedDate = dpCreatedDate.Date;
            dateChanged = true;
        }

        private async void btnSaveProject_Clicked(object sender, EventArgs e)
        {
            if (editing)
            {
                project.PrimaryCollector = (entryPrimaryCollectorProject.Text is null) ? "" : entryPrimaryCollectorProject.Text;
                project.CreatedDate = dateChanged ? dpCreatedDate.Date : project.CreatedDate;

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

            // check to make sure name is present
            if (entryProjectName.Text is null || entryProjectName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Project name is required.");
                return;
            }

            project.ProjectName = entryProjectName.Text;
            project.PrimaryCollector = entryPrimaryCollectorProject.Text;
            project.CreatedDate = dpCreatedDate.Date;

            // check for duplicate names first
            if (ORM.CheckExists(project))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert($"'{project.ProjectName}' already exists. Enter a Unique name for a new.");
                return;
            }

            // save project to database
            int autoKeyResult = ORM.GetConnection().Insert(project);
            Debug.WriteLine("inserted project, recordno is: " + autoKeyResult.ToString());

            // DependencyService.Get<ICrossPlatformToast>().ShortAlert("Project " + project.ProjectName + " saved");

            // automatically navigate to the collecting page after saving the project
            await Navigation.PushAsync(new CollectingPage(project));
        }

        private void btnNewProject_Clicked(object sender, EventArgs e)
        {
            project = new Project();
            LoadDefaults();
            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Project");
        }
    }
}
