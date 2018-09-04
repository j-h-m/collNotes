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
        private string projectName = "";
        private string primaryCollector = "";
        private DateTime createdDate;

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
            projectName = ((Entry)sender).Text;
        }

        private void entryPrimaryCollectorProject_Completed(object sender, EventArgs e)
        {
            primaryCollector = ((Entry)sender).Text;
        }

        private void dpCreatedDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            createdDate = ((DatePicker)sender).Date;
        }

        private void btnSaveProject_Clicked(object sender, EventArgs e)
        {
            // check to make sure all data is present
            project.ProjectName = projectName;
            project.PrimaryCollector = primaryCollector;
            project.CreatedDate = createdDate;

            // save project to database
            LiteDB.Connection.CreateTable<Project>(); // does 'create if not exist'
            int autoIncRecordNo = LiteDB.Connection.Insert(project);
            Console.WriteLine("inserted project recordno: " + autoIncRecordNo.ToString());
        }

        private async Task btnBack_ClickedAsync(object sender, EventArgs e)
        {
            // back to main page
            // don't need this for navigation but will for modal pages
            await Navigation.PopAsync();
        }
    }
}