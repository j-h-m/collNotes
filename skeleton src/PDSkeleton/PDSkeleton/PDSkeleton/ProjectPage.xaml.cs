using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * Create Project Page
 * User Creates a Project and saves it which causes a record to be written to the SQLite file
 * All saved projects can be read from DB as Project object
 * 
 */ 

namespace PDSkeleton
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjectPage : ContentPage
	{
        private Project project;

		public ProjectPage ()
		{
            project = new Project();
			InitializeComponent();
		}

        public ProjectPage (Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        private void entryProjectName_Completed(object sender, EventArgs e)
        {
            project.ProjectName = ((Entry)sender).Text;
        }

        private void entryPrimaryCollectorProject_Completed(object sender, EventArgs e)
        {
            project.PrimaryCollector = ((Entry)sender).Text;
        }

        private void dpCreatedDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            project.CreatedDate = ((DatePicker)sender).Date;
        }

        private void btnSaveProject_Clicked(object sender, EventArgs e)
        {
            // check to make sure all data is present
            if (entryProjectName.Text is null || entryPrimaryCollectorProject.Text is null)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter all information for Project");
                return;
            }

            project.ProjectName = entryProjectName.Text;
            project.PrimaryCollector = entryPrimaryCollectorProject.Text;
            project.CreatedDate = dpCreatedDate.Date;

            // save project to database
            ORM.GetConnection().CreateTable<Project>();
            int autoKeyResult = ORM.GetConnection().Insert(project);
            Debug.WriteLine("inserted project, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Project " + project.ProjectName + " saved!");
        }
    }
}