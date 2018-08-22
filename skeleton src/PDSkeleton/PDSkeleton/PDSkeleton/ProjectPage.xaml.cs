using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            //project.CreatedDate = dpCreatedDate.Date;
        }

        private void btnSaveProject_Clicked(object sender, EventArgs e)
        {
            // check to make sure all data is present

            // save project to database
            var db = ORM.GetConnection();
            db.CreateTable<Project>(); // does 'create if not exist'
            db.Insert(project);

            var tempTable = db.Table<Project>();
            foreach (var item in tempTable)
            {
                //Console.WriteLine("project: " + item.ProjectName);
            }
        }

        private async Task btnBack_ClickedAsync(object sender, EventArgs e)
        {
            // back to main page
            // don't need this for navigation but will for modal pages
            await Navigation.PopAsync();
        }
    }
}