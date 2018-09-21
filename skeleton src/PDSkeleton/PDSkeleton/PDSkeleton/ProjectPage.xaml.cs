using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDSkeleton
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjectPage : ContentPage
	{
        private Project project;

		public ProjectPage ()
		{
            project = new Project();
			InitializeComponent ();
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
            if (project.ProjectName is null || project.PrimaryCollector is null)
            {
                
            }

            // save project to database
            SQLite.SQLiteConnection connection = ORM.GetConnection();
            connection.CreateTable<Project>(); // does 'create if not exist'
            int autoKeyResult = connection.Insert(project);
            Console.WriteLine("inserted project, recordno is: " + autoKeyResult.ToString());
        }

        private async Task btnBack_ClickedAsync(object sender, EventArgs e)
        {
            // back to main page
            // don't need this for navigation but will for modal pages
            await Navigation.PopAsync();
        }
    }
}