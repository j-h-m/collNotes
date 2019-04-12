using System;
using System.Diagnostics;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;

namespace collnotes
{
    /// <summary>
    /// Project page.
    /// </summary>
    public partial class ProjectPage : ContentPage
    {
        // hold object instance for current Page
        private Project project;

        // flags used to control what happens in some events
        private bool userIsEditing = false;
        private bool dateChanged = false;
        private bool editWasSaved = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.ProjectPage"/> class.
        /// Create Project no-args constructor.
        /// </summary>
        public ProjectPage()
        {
            project = new Project();
            InitializeComponent();
            // the reason this is called after InitializedComponent:
            //  - components of the Page cannot be modified until they are created.
            LoadDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.ProjectPage"/> class.
        /// Edit Project constructor. Takes the Project to edit as an argument.
        /// </summary>
        /// <param name="project">Project.</param>
        public ProjectPage(Project project)
        {
            this.project = project;
            InitializeComponent();

            entryProjectName.Text = project.ProjectName;
            entryPrimaryCollectorProject.Text = project.PrimaryCollector;
            dpCreatedDate.Date = project.CreatedDate;
            entryProjectName.IsEnabled = false;
            btnNewProject.IsEnabled = false;
            btnNewProject.IsVisible = false;

            userIsEditing = true;
            editWasSaved = false;

            // Hide the back button and only show Update/Cancel buttons for user
            NavigationPage.SetHasBackButton(this, false);
            btnBack.IsVisible = userIsEditing; // by default the button is visible, this is here for clarity
        }

        /// <summary>
        /// Loads the default values for controls with corresponding App Variables.
        /// </summary>
        private void LoadDefaults()
        {
            entryPrimaryCollectorProject.Text = (AppVariables.CollectorName is null) ? "" : AppVariables.CollectorName;
            entryProjectName.Text = "Project-" + (DataFunctions.GetAllProjectsCount() + 1).ToString();
            dpCreatedDate.Date = DateTime.Today;
            btnBack.IsVisible = userIsEditing;
        }

        /// <summary>
        /// Event handler for the dpCreatedDate DatePicker.
        /// Sets the current Project's created date.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void dpCreatedDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            project.CreatedDate = dpCreatedDate.Date;
            dateChanged = true;
        }

        /// <summary>
        /// btnSaveProject Click event.
        /// Handles the Saving, or Updating, of the current Project.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private async void btnSaveProject_Clicked(object sender, EventArgs e)
        {
            if (userIsEditing)
            {
                project.PrimaryCollector = (entryPrimaryCollectorProject.Text is null) ? "" : entryPrimaryCollectorProject.Text;
                project.CreatedDate = dateChanged ? dpCreatedDate.Date : project.CreatedDate;

                int updateResult = DatabaseFile.GetConnection().Update(project, typeof(Project));
                if (updateResult == 1)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(project.ProjectName + " update succeeded.");
                    editWasSaved = true;
                    return;
                }
                else
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert(project.ProjectName + " update failed.");
                    return;
                }
            }

            SaveCurrentProject();

            // automatically navigate to the collecting page after saving the project
            await Navigation.PushAsync(new CollectingPage(project));
            Navigation.RemovePage(this);
        }

        /// <summary>
        /// btnNewProject Click event.
        /// Resets the current ProjectPage for a new Project.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void btnNewProject_Clicked(object sender, EventArgs e)
        {
            project = new Project();
            LoadDefaults();
            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Cleared for new Project");
        }

        /// <summary>
        /// btnBack Click event.
        /// Provides the user with the option to exit an Update page.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private async void btnBack_Clicked(object sender, EventArgs e)
        {
            if (editWasSaved)
            {
                Navigation.RemovePage(this);
                return;
            }

            bool response = await DisplayAlert("Are you sure?", "Are you sure you don't want to save your changes?", "Yes", "No");
            if (response)
                Navigation.RemovePage(this);
        }

        /// <summary>
        /// Saves the current project.
        /// </summary>
        /// <returns><c>true</c>, if current project was saved, <c>false</c> otherwise.</returns>
        private bool SaveCurrentProject()
        {
            // check to make sure name is present
            if (entryProjectName.Text is null || entryProjectName.Text.Equals(""))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Project name is required.");
                return false;
            }

            project.ProjectName = entryProjectName.Text;
            project.PrimaryCollector = entryPrimaryCollectorProject.Text;
            project.CreatedDate = dpCreatedDate.Date;

            // check for duplicate names first
            if (DataFunctions.CheckExists(project, project.ProjectName))
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert($"'{project.ProjectName}' already exists...");
                return false;
            }

            // save project to database
            int autoKeyResult = DatabaseFile.GetConnection().Insert(project);
            Debug.WriteLine("inserted project, recordno is: " + autoKeyResult.ToString());

            return true;
        }
    }
}
