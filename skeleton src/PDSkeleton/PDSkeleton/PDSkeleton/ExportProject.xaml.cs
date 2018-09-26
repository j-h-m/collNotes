using System;
using System.IO;
using System.Collections.Generic;

using Plugin.ShareFile;

using Xamarin.Forms;

/*
 * User chooses a project and can then export all Site/Specimen data to a CSV file for sharing.
 * CSV made manually following - https://tools.ietf.org/html/rfc4180
 * 
 */ 

namespace PDSkeleton
{
    public partial class ExportProject : ContentPage
    {
        private Project selectedProject = null;
        private List<Project> projectList;
        private string crlf = Environment.NewLine;

        public ExportProject()
        {
            InitializeComponent();

            projectList = ORM.GetConnection().Query<Project>("select * from Project");

            List<string> projectNameList = new List<string>();

            foreach (Project p in projectList)
            {
                projectNameList.Add(p.ProjectName);
            }

            pickerExportProject.ItemsSource = projectNameList;
        }

        public void pickerExportProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (Project p in projectList)
                {
                    if (p.ProjectName.Equals(pickerExportProject.SelectedItem.ToString()))
                    {
                        selectedProject = p;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;
            }
        }

        public void btnExportProjectCSV_Clicked(object sender, EventArgs e)
        {
            if (selectedProject == null)
            {
                // no project selected
                return;
            }

            string filePath = "";

            // site # - specimen #
            // site # - # (not specimen record)

            string selectedProjectName = selectedProject.ProjectName;

            // get Trips for selected Project
            List<Trip> selectedProjectTrips = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + selectedProjectName + "'");

            List<string> selectedProjectTripNames = new List<string>();

            foreach (Trip t in selectedProjectTrips)
            {
                selectedProjectTripNames.Add(t.TripName);
            }

            // get Sites for each Trip
            Dictionary<string, List<Site>> sitesForTrips = new Dictionary<string, List<Site>>();

            foreach (string s in selectedProjectTripNames)
            {
                List<Site> sites = ORM.GetConnection().Query<Site>("select * from Site where TripName = '" + s + "'");
                sitesForTrips.Add(s, sites);
            }

            // get Specimen for each Site
            Dictionary<string, List<Specimen>> specimenForSites = new Dictionary<string, List<Specimen>>();

            foreach (var trip in sitesForTrips) // trip, list<site>
            {
                foreach (var site in trip.Value) // go through list<site>
                {
                    List<Specimen> specimenList = ORM.GetConnection().Query<Specimen>("select * from Specimen where SiteName = '" + site.SiteName + "'");
                    specimenForSites.Add(site.SiteName, specimenList);
                }
            }

            // create CSV with site # - specimen # for each specimen record
            // start with 1 - 1

            string csvContent = "";

            // 28 column header
            csvContent += "otherCatalogNumbers,siteNumber,specimenNumber,genericcolumn2,associatedCollectors,habitat,individualCount,reproductiveCondition,locality,locationRemarks,occurrenceRemarks,recordedBy," +
                "samplingEffort,substrate,associatedTaxa,eventDate,establishmentMeans,genericcolumn1,decimalLatitude,decimalLongitude,coordinateUncertaintyInMeters,minimumElevationInMeters," +
                "scientificName,scientificNameAuthorship,country,stateProvince,county,path" + crlf;

            foreach (var item in specimenForSites)
            {
                string siteName = item.Key;
                List<Specimen> specimenList = item.Value;

                foreach (var specimen in specimenList)
                {
                    
                }
            }

            File.WriteAllText(filePath + "/" + selectedProject.ProjectName + DateTime.Now.ToShortDateString() + ".csv", csvContent, System.Text.Encoding.UTF8); // create csvfile with utf8 encoding

            CrossShareFile.Current.ShareLocalFile(filePath, "Share Specimen Export");
        }
    }


}
