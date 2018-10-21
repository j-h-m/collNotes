﻿using System;
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

        // no args constructor
		public ProjectPage ()
		{
            project = new Project();
			InitializeComponent();
		}

        // constructor accepts selected project from MainPage
        public ProjectPage (Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        // project name text entry event
        private void entryProjectName_Completed(object sender, EventArgs e)
        {
            project.ProjectName = ((Entry)sender).Text;
        }

        // primary collector text entry event
        private void entryPrimaryCollectorProject_Completed(object sender, EventArgs e)
        {
            project.PrimaryCollector = ((Entry)sender).Text;
        }

        // date picker created date event
        private void dpCreatedDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            project.CreatedDate = ((DatePicker)sender).Date;
        }

        // SaveProject button event
        //  - checks for required data on click
        //  - writes Project to the local database
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
    }
}