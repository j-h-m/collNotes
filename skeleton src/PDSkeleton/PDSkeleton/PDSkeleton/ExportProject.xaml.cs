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

            // data from project
            string recordedBy = selectedProject.PrimaryCollector;
            string samplingEffort = selectedProject.ProjectName;

            // get Trips for selected Project
            List<Trip> selectedProjectTrips = ORM.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + selectedProject.ProjectName + "'");

            // get Sites for each Trip
            Dictionary<string, List<Site>> sitesForTrips = new Dictionary<string, List<Site>>();

            foreach (Trip trip in selectedProjectTrips)
            {
                List<Site> sites = ORM.GetConnection().Query<Site>("select * from Site where TripName = '" + trip.TripName + "'");
                sitesForTrips.Add(trip.TripName, sites);
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
            // use recordno to get site # and specimen #

            string csvContent = "";

            // 28 column header
            csvContent += "otherCatalogNumbers,siteNumber,specimenNumber,genericcolumn2,associatedCollectors,habitat,individualCount,reproductiveCondition,locality,locationRemarks,occurrenceRemarks,recordedBy," +
                "samplingEffort,substrate,associatedTaxa,eventDate,establishmentMeans,genericcolumn1,decimalLatitude,decimalLongitude,coordinateUncertaintyInMeters,minimumElevationInMeters," +
                "scientificName,scientificNameAuthorship,country,stateProvince,county,path" + crlf;

            foreach (KeyValuePair<string, List<Specimen>> sitesSpecimen in specimenForSites)
            {
                // get site, trip info using the site name
                string siteName = sitesSpecimen.Key;
                string tripName = "";

                Site refSite = null;

                foreach (KeyValuePair<string, List<Site>> sitesTrip in sitesForTrips)
                {
                    foreach (Site site in sitesTrip.Value)
                    {
                        if (siteName == site.SiteName)
                        {
                            refSite = site;
                            tripName = sitesTrip.Key;
                            break;
                        }
                    }
                    if (!tripName.Equals(""))
                    {
                        break;
                    }
                }

                Trip trip = null;
                foreach (Trip t in selectedProjectTrips)
                {
                    if (tripName.Equals(t.TripName))
                    {
                        trip = t;
                        break;
                    }
                }

                // trip level data
                string assColl = "";
                DateTime eventDate = DateTime.Now;

                if (trip != null)
                {
                    assColl = trip.AdditionalCollectors;
                    eventDate = trip.CollectionDate;
                }

                // site level data
                string habitat = refSite.Habitat;
                string locality = refSite.Locality;
                string locationNotes = refSite.LocationNotes;
                string associatedTaxa = refSite.AssociatedTaxa;
                int siteNumber = refSite.RecordNo;

                // specimen level data
                foreach (Specimen spec in sitesSpecimen.Value)
                {
                    int specimenNumber = spec.RecordNo;
                    string genericColumn2 = spec.AdditionalInfo;
                    string individualCount = spec.IndividualCount;
                    string reproductiveCondition = spec.LifeStage;
                    string occurrenceRemarks = spec.OccurrenceNotes;
                    string substrate = spec.Substrate;
                    string establishmentMeans = (spec.Cultivated) ? "cultivated" : "";
                    string genericColumn1 = spec.FieldIdentification;
                    string latitude = spec.GPSCoordinates.Split(',')[0];
                    string longitude = spec.GPSCoordinates.Split(',')[1];
                    string coordinateUncertaintyMeters = spec.GPSCoordinates.Split(',')[2];
                    string minimumElevationMeters = spec.GPSCoordinates.Split(',')[3];

                    csvContent += siteNumber.ToString() + "-" + specimenNumber.ToString() + "," +   // other catalog numbers
                                            siteNumber.ToString() + "," +                           // site number
                                            specimenNumber.ToString() + "," +                       // specimen number
                                            genericColumn2 + "," +                                  // generic column 2 (additional info)
                                            assColl + "," +                                         // associated collectors
                                            habitat + "," +                                         // habitat
                                            individualCount + "," +                                 // individual count
                                            reproductiveCondition + "," +                           // reproductive condition
                                            locality + "," +                                        // locality
                                            locationNotes + "," +                                   // location remarks
                                            occurrenceRemarks + "," +                               // occurrence remarks
                                            recordedBy + "," +                                      // recorded by (primary collector)
                                            samplingEffort + "," +                                  // sampling effort (project name)
                                            substrate + "," +                                       // substrate
                                            associatedTaxa + "," +                                  // associated taxa
                                            eventDate.ToString("yyyy-MM-dd") + "," +                // event date
                                            establishmentMeans + "," +                              // establishment means (cultivated)
                                            genericColumn1 + "," +                                  // generic column 1 (field identification)
                                            latitude + "," +                                        // latitude
                                            longitude + "," +                                       // longitude
                                            coordinateUncertaintyMeters + "," +                     // error in Meters
                                            minimumElevationMeters +                                // elevation
                                            "," + "," + "," + "," + "," + ",";                      // 6 empty columns for desktop determinations
                }

                string filePath = "";

                filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/" + selectedProject.ProjectName + DateTime.Now.ToString("MM-dd-yyyy") + ".csv";

                File.WriteAllText(filePath, csvContent, System.Text.Encoding.UTF8); // create csvfile with utf8 encoding

                CrossShareFile.Current.ShareLocalFile(filePath, "Share Specimen Export"); // this isn't working
                // https://github.com/nielscup/ShareFile
            }
        }
    }
}