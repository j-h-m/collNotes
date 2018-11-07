using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace PDSkeleton
{
    public partial class EditPage : ContentPage
    {
        private Project project;

        // no args constructor for edit page
        public EditPage()
        {
            InitializeComponent();
        }

        // constructor for edit page
        // takes project object as argument to edit
        public EditPage(Project project)
        {
            this.project = project;
            InitializeComponent();
        }

        // edit project button event
        // navigate to project page and pass current project for editing
        public async void btnEditProject_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProjectPage(project));
        }

        // edit trip button event
        // bring up selection of trips for projecg
        // pass selected to TripPage for editing
        public async void btnEditTrip_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + project.ProjectName + "'");

                string[] trips = new string[tripList.Count];
                for (int i = 0; i < trips.Length; i++)
                {
                    trips[i] = tripList[i].ProjectName;
                }

                var action = await DisplayActionSheet("Choose a Trip", "Cancel", null, trips);

                foreach (Trip t in tripList)
                {
                    if (t.TripName == action)
                    {
                        await Navigation.PushAsync(new TripPage(t));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Are there any Trips for this Project?");
            }
        }

        public async void btnEditSite_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + project.ProjectName + "'");

                List<Site> siteList = new List<Site>();

                foreach (Trip trip in tripList)
                {
                    siteList.AddRange(ORM.GetConnection().Query<Site>("select * from Site where TripName = '" + trip.TripName + "'"));
                }

                string[] sites = new string[siteList.Count];
                for (int i = 0; i < sites.Length; i++)
                {
                    sites[i] = siteList[i].SiteName;
                }

                var action = await DisplayActionSheet("Choose a Site", "Cancel", null, sites);

                foreach (Site s in siteList)
                {
                    if (s.SiteName == action)
                    {
                        await Navigation.PushAsync(new SitePage(s));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Are there any Sites for this Project?");
            }
        }

        public async void btnEditSpecimen_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<Trip> tripList = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + project.ProjectName + "'");

                List<Site> siteList = new List<Site>();

                foreach (Trip trip in tripList)
                {
                    siteList.AddRange(ORM.GetConnection().Query<Site>("select * from Site where TripName = '" + trip.TripName + "'"));
                }

                List<Specimen> specimenList = new List<Specimen>();

                foreach (Site site in siteList)
                {
                    specimenList.AddRange(ORM.GetConnection().Query<Specimen>("select * from Specimen where SiteName = '" + site.SiteName + "'"));
                }

                string[] specimens = new string[specimenList.Count];
                for (int i = 0; i < specimens.Length; i++)
                {
                    specimens[i] = specimenList[i].FieldIdentification;
                }

                var action = await DisplayActionSheet("Choose a Specimen", "Cancel", null, specimens);

                foreach (Specimen s in specimenList)
                {
                    if (s.FieldIdentification == action)
                    {
                        await Navigation.PushAsync(new SpecimenPage(s));
                        break;
                    }
                }
            } 
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Are there any Specimen for this Project?");
            }
        }
    }
}
