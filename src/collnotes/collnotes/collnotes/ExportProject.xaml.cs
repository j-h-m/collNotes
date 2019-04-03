using System;
using System.IO;
using System.Collections.Generic;
using Plugin.ShareFile;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using System.Diagnostics;
using collnotes.Data;
using collnotes.Interfaces;
using System.Linq;

using Xamarin.Essentials;

// This one needs some LINQ...

namespace collnotes
{
    /// <summary>
    /// Export project.
    /// </summary>
    public partial class ExportProject : ContentPage
    {
        private Project selectedProject;
        private List<Project> projectList;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.ExportProject"/> class.
        /// </summary>
        public ExportProject()
        {
            InitializeComponent();

            projectList = DataFunctions.GetProjects();

            List<string> projectNames = (from el in projectList
                                            select el.ProjectName).ToList();

            pickerExportProject.ItemsSource = projectNames;
        }

        /// <summary>
        /// Pickers the export project selected index changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void pickerExportProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedProject = (from el in projectList
                                   where el.ProjectName == pickerExportProject.SelectedItem.ToString()
                                   select el).First();                                  

                if (!(selectedProject is null))
                {
                    await CSVExport_Helper();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Buttons the export project csv clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void btnExportProjectCSV_Clicked(object sender, EventArgs e)
        {
            await CSVExport_Helper();
        }

        /// <summary>
        /// CSV the export helper.
        /// </summary>
        /// <returns>The xport helper.</returns>
        private async Task CSVExport_Helper()
        {
            var result = await CheckExternalFilePermissions();

            if (!result)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Storage permission required for data export!");
            }

            try
            {
                // get Trips for selected Project
                List<Trip> selectedProjectTrips = DataFunctions.GetTrips(selectedProject.ProjectName);

                // get Sites for each Trip
                Dictionary<string, List<Site>> sitesForTrips = new Dictionary<string, List<Site>>();

                foreach (Trip trip in selectedProjectTrips)
                {
                    List<Site> sites = DataFunctions.GetSites(trip.TripName);
                    sitesForTrips.Add(trip.TripName, sites);
                }

                if (sitesForTrips.Count == 0)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No trips.");
                    return;
                }

                // get Specimen for each Site
                Dictionary<string, List<Specimen>> specimenForSites = new Dictionary<string, List<Specimen>>();

                foreach (var trip in sitesForTrips)
                { // trip, list<site>
                    foreach (var site in trip.Value)
                    { // go through list<site>
                        List<Specimen> specimenList = DataFunctions.GetSpecimen(site.SiteName);
                        specimenForSites.Add(site.SiteName, specimenList);
                    }
                }

                if (specimenForSites.Count == 0)
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("No sites.");
                    return;
                }

                // csv content string to write to file
                string csvContent = "";

                switch (AppVariables.DataExportFormat)
                {
                    case "Darwin Core":
                        csvContent = CSVExport.CreateCSVForExport(selectedProject, CSVExport.DataExportType.DarwinCore, selectedProjectTrips, specimenForSites, sitesForTrips);
                        break;
                    default:
                        csvContent = CSVExport.CreateCSVForExport(selectedProject, CSVExport.DataExportType.DarwinCore, selectedProjectTrips, specimenForSites, sitesForTrips);
                        break;
                }

                string filePath = DependencyService.Get<ICrossPlatform_GetShareFolder>().GetShareFolder();

                // save to local app data
                // to share in email must use temporary file, can't use internal storage
                string fileName = selectedProject.ProjectName.Trim() + ".csv";

                string localFileLocation = Path.Combine(filePath, fileName);

                File.WriteAllText(localFileLocation, csvContent, System.Text.Encoding.UTF8); // create csvfile with utf8 encoding, in permanent local storage

                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    await SendEmail("Project Export", "Set your recipients and send the export data...", new List<string>(), localFileLocation);
                }
                else
                {
                    CrossShareFile.Current.ShareLocalFile(localFileLocation, "Share Specimen Export");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Export failed.");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Uses email to allow the User to export their data.
        /// Automatically adds the file as an attachment.
        /// Only used on iOS.
        /// </summary>
        /// <returns>The email.</returns>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        /// <param name="recipients">Recipients.</param>
        /// <param name="filepath">Filepath.</param>
        public async Task SendEmail(string subject, string body, List<string> recipients, string filepath)
        {
            try
            {
                ExperimentalFeatures.Enable("EmailAttachments_Experimental"); // required to allow attachment in email
                List<EmailAttachment> emailAttachments = new List<EmailAttachment>();
                EmailAttachment attachment = new EmailAttachment(filepath);
                emailAttachments.Add(attachment);

                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    Attachments = emailAttachments
                };
                await Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Checks the external file permissions.
        /// </summary>
        /// <returns>The external file permissions.</returns>
        public async Task<bool> CheckExternalFilePermissions()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    return false;
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                {
                    status = results[Permission.Storage];
                }
            }

            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else if (status != PermissionStatus.Unknown)
            {
                return false;
            }

            return false;
        }
    }
}
