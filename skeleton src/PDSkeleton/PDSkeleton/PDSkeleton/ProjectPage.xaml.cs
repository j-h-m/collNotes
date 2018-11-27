using System;
using System.Diagnostics;
using System.Collections.Generic;
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
        private List<Project> existingProjects;

        private bool editing = false;

        // constructor for collecting
		public ProjectPage ()
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
        private void btnSaveProject_Clicked(object sender, EventArgs e)
        {
            if (editing)
            {
                if (!(entryPrimaryCollectorProject.Text is null) && !entryPrimaryCollectorProject.Text.Equals(""))
                {
                    project.PrimaryCollector = entryPrimaryCollectorProject.Text;
                    project.CreatedDate = dpCreatedDate.Date;

                    int updateResult = ORM.GetConnection().Update(project, typeof(Project));
                    if (updateResult == 1) // what is result of above call?
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(project.ProjectName + " save succeeded.");
                        return;
                    }
                    else
                    {
                        DependencyService.Get<ICrossPlatformToast>().ShortAlert(project.ProjectName + " save failed.");
                        return;
                    }
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Need all info to save Project!");
                    return;
                }
            }

            // check to make sure all data is present
            if (entryProjectName.Text is null || entryProjectName.Text.Equals("") || 
                entryPrimaryCollectorProject.Text is null || entryPrimaryCollectorProject.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Must enter all information for Project!");
                return;
            }

            project.ProjectName = entryProjectName.Text;
            project.PrimaryCollector = entryPrimaryCollectorProject.Text;
            project.CreatedDate = dpCreatedDate.Date;

            ORM.GetConnection().CreateTable<Project>();

            // check for duplicate names first
            existingProjects = ORM.GetConnection().Query<Project>("select * from Project");

            foreach (Project p in existingProjects)
            {
                if (p.ProjectName.Equals(project.ProjectName))
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("You already have a project with the same name!");
                    return;
                }
            }

            // save project to database
            int autoKeyResult = ORM.GetConnection().Insert(project);
            Debug.WriteLine("inserted project, recordno is: " + autoKeyResult.ToString());

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Project " + project.ProjectName + " saved!");
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