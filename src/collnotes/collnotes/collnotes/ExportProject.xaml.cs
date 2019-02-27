using System;
using System.IO;
using System.Collections.Generic;
using Plugin.ShareFile;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace collnotes
{
    public partial class ExportProject : ContentPage
    {
        private Project selectedProject;
        private List<Project> projectList;

        public enum DataExportType
        {
            DarwinCore
        }

        public ExportProject()
        {
            InitializeComponent();
            projectList = ORM.GetProjects();

            List<string> projectNameList = new List<string>();

            foreach (Project p in projectList) {
                projectNameList.Add(p.ProjectName);
            }

            pickerExportProject.ItemsSource = projectNameList;
        }

        public async void pickerExportProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (Project p in projectList) {
                    if (p.ProjectName.Equals(pickerExportProject.SelectedItem.ToString())) {
                        selectedProject = p;
                        break;
                    }
                }

                if (!(selectedProject is null)) {
                    await CSVExport_Helper();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async void btnExportProjectCSV_Clicked(object sender, EventArgs e)
        {
            await CSVExport_Helper();
        }

        private async Task CSVExport_Helper()
        {
            var result = await CheckExternalFilePermissions();

            if (!result) {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Storage permission required for data export!");
            }

            try
            {
                if (selectedProject == null) {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("Select a Project first");
                    return;
                }

                // get Trips for selected Project
                List<Trip> selectedProjectTrips = ORM.GetTrips(selectedProject.ProjectName);

                // get Sites for each Trip
                Dictionary<string, List<Site>> sitesForTrips = new Dictionary<string, List<Site>>();

                foreach (Trip trip in selectedProjectTrips) {
                    List<Site> sites = ORM.GetSites(trip.TripName);
                    sitesForTrips.Add(trip.TripName, sites);
                }

                if (sitesForTrips.Count == 0) {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No trips.");
                    return;
                }

                // get Specimen for each Site
                Dictionary<string, List<Specimen>> specimenForSites = new Dictionary<string, List<Specimen>>();

                foreach (var trip in sitesForTrips) { // trip, list<site>
                    foreach (var site in trip.Value) { // go through list<site>
                        List<Specimen> specimenList = ORM.GetSpecimen(site.SiteName);
                        specimenForSites.Add(site.SiteName, specimenList);
                    }
                }

                if (specimenForSites.Count == 0) {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No sites.");
                    return;
                }

                // csv content string to write to file
                string csvContent = "";

                switch (AppVariables.DataExportFormat) {
                    case "Darwin Core":
                        csvContent = CreateCSVForExport(DataExportType.DarwinCore, selectedProjectTrips, specimenForSites, sitesForTrips);
                        break;
                    default:
                        csvContent = CreateCSVForExport(DataExportType.DarwinCore, selectedProjectTrips, specimenForSites, sitesForTrips);
                        break;
                }

                string filePath = DependencyService.Get<ICrossPlatform_GetShareFolder>().GetShareFolder();

                // save to local app data
                // to share in email must use temporary file, can't use internal storage
                string fileName = selectedProject.ProjectName.Trim() + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".csv";

                string localFileLocation = Path.Combine(filePath, fileName);

                File.WriteAllText(localFileLocation, csvContent, System.Text.Encoding.UTF8); // create csvfile with utf8 encoding, in permanent local storage

                CrossShareFile.Current.ShareLocalFile(localFileLocation, "Share Specimen Export"); // working on Android, not showing all sharing options on iOS... https://github.com/nielscup/ShareFile
            }
            catch (Exception ex)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Export failed.");
                Debug.WriteLine(ex.Message);
            }
        }

        public string CreateCSVForExport(DataExportType det, List<Trip> selectedProjectTrips, Dictionary<string, List<Specimen>> specimenForSites, Dictionary<string, List<Site>> sitesForTrips)
        {
            // create CSV with site # - specimen # for each specimen record
            // use recordno to get site # and specimen #

            string csvContent = "";

            // call function to generate csv data based on format
            switch (det) {
                case DataExportType.DarwinCore:
                    csvContent = GenDarwinCore(selectedProjectTrips, specimenForSites, sitesForTrips);
                    break;
            }

            return csvContent;
        }

        private string GenDarwinCore(List<Trip> selectedProjectTrips, Dictionary<string, List<Specimen>> specimenForSites, Dictionary<string, List<Site>> sitesForTrips)
        {
            string csvContent = "";

            // 27 column header -- changed samplingEffort ---> Label Project --- added samplingEffort back for project date info
            csvContent += "siteNumber,specimenNumber,genericcolumn2,associatedCollectors,habitat,individualCount,reproductiveCondition,locality,locationRemarks,occurrenceRemarks,recordedBy," +
                "Label Project,samplingEffort,substrate,associatedTaxa,eventDate,establishmentMeans,genericcolumn1,decimalLatitude,decimalLongitude,coordinateUncertaintyInMeters,minimumElevationInMeters," +
                "scientificName,scientificNameAuthorship,country,stateProvince,county,path" + Environment.NewLine;

            foreach (KeyValuePair<string, List<Specimen>> sitesSpecimen in specimenForSites) {
                // get site, trip info using the site name
                string siteName = sitesSpecimen.Key;
                string tripName = "";

                Site refSite = ORM.GetSiteByName(siteName);
                Trip trip = ORM.GetTripByName(refSite.TripName);
                tripName = trip.TripName;

                // project level data
                string recordedBy = selectedProject.PrimaryCollector;
                string samplingEffort = selectedProject.ProjectName + "," + selectedProject.CreatedDate.ToString("yyyy-MM-dd");
                string labelProject = selectedProject.ProjectName;

                // trip level data
                string assColl = trip.AdditionalCollectors;
                DateTime eventDate = trip.CollectionDate;

                // site level data
                string habitat = refSite.Habitat;
                string locality = refSite.Locality;
                string locationNotes = refSite.LocationNotes;
                string associatedTaxa = refSite.AssociatedTaxa;
                int siteNumber = refSite.RecordNo;

                // specimen level data
                // foreach (Specimen spec in sitesSpecimen.Value)
                for (int i = -1; i < sitesSpecimen.Value.Count; i++) {
                    int specimenNumber;
                    string genericColumn2, individualCount, reproductiveCondition, occurrenceRemarks, substrate, establishmentMeans, genericColumn1, latitude, longitude, coordinateUncertaintyMeters, minimumElevationMeters;
                    if (i == -1) { // add the site # record
                        specimenNumber = -1; // should match a collector's desired collection count*
                        genericColumn2 = "";
                        individualCount = "";
                        reproductiveCondition = "";
                        occurrenceRemarks = "";
                        substrate = "";
                        establishmentMeans = "";
                        genericColumn1 = "";
                        latitude = (!refSite.GPSCoordinates.Equals("")) ? refSite.GPSCoordinates.Split(',')[0] : "";
                        longitude = (!refSite.GPSCoordinates.Equals("")) ? refSite.GPSCoordinates.Split(',')[1] : "";
                        coordinateUncertaintyMeters = (!refSite.GPSCoordinates.Equals("")) ? refSite.GPSCoordinates.Split(',')[2] : "";
                        minimumElevationMeters = (!refSite.GPSCoordinates.Equals("")) ? minimumElevationMeters = refSite.GPSCoordinates.Split(',')[3] : "";
                    } else {
                        Specimen spec = sitesSpecimen.Value[i];
                        specimenNumber = spec.SpecimenNumber; // should match a collector's desired collection count*
                        genericColumn2 = spec.AdditionalInfo;
                        individualCount = spec.IndividualCount;
                        reproductiveCondition = spec.LifeStage;
                        occurrenceRemarks = spec.OccurrenceNotes;
                        substrate = spec.Substrate;
                        establishmentMeans = (spec.Cultivated) ? "cultivated" : "";
                        genericColumn1 = spec.FieldIdentification;
                        latitude = (!spec.GPSCoordinates.Equals("")) ? spec.GPSCoordinates.Split(',')[0] : "";
                        longitude = (!spec.GPSCoordinates.Equals("")) ? spec.GPSCoordinates.Split(',')[1] : "";
                        coordinateUncertaintyMeters = (!spec.GPSCoordinates.Equals("")) ? spec.GPSCoordinates.Split(',')[2] : "";
                        minimumElevationMeters = (!spec.GPSCoordinates.Equals("")) ? minimumElevationMeters = spec.GPSCoordinates.Split(',')[3] : "";
                    }

                    csvContent += "\"" + siteNumber.ToString() + "\",\"" +                              // site number
                    ((specimenNumber == -1) ? "#" : specimenNumber.ToString()) + "\",\"" +              // specimen number
                                            genericColumn2 + "\",\"" +                                  // generic column 2 (additional info)
                                            assColl + "\",\"" +                                         // associated collectors
                                            habitat + "\",\"" +                                         // habitat
                                            individualCount + "\",\"" +                                 // individual count
                                            reproductiveCondition + "\",\"" +                           // reproductive condition
                                            locality + "\",\"" +                                        // locality
                                            locationNotes + "\",\"" +                                   // location remarks
                                            occurrenceRemarks + "\",\"" +                               // occurrence remarks
                                            recordedBy + "\",\"" +                                      // recorded by (primary collector)
                                            labelProject + "\",\"" +                                    // Label Project (project name)
                                            samplingEffort + "\",\"" +                                  // sampling effort (project name, created date)
                                            substrate + "\",\"" +                                       // substrate
                                            associatedTaxa + "\",\"" +                                  // associated taxa
                                            eventDate.ToString("yyyy-MM-dd") + "\",\"" +                // event date - ISO Format*
                                            establishmentMeans + "\",\"" +                              // establishment means (cultivated)
                                            genericColumn1 + "\",\"" +                                  // generic column 1 (field identification)
                                            latitude + "\",\"" +                                        // latitude
                                            longitude + "\",\"" +                                       // longitude
                                            coordinateUncertaintyMeters + "\",\"" +                     // error in Meters
                                            minimumElevationMeters +                                    // elevation
                                            "\"," + "," + "," + "," + "," + "," + Environment.NewLine;  // 6 empty columns for desktop determinations
                } // wrap fields in double quotes and test with user input fields including commas and try single quotes
            }

            return csvContent;
        }

        public async Task<bool> CheckExternalFilePermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted) {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage)) {
                    return false;
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
            }

            if (status == PermissionStatus.Granted) {
                return true;
            } else if (status != PermissionStatus.Unknown) {
                return false;
            }

            return false;
        }
    }
}
