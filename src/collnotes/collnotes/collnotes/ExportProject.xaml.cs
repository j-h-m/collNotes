using collnotes.Data;
using collnotes.Data.ExportFormat;
using collnotes.Interfaces;
using CsvHelper;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.ShareFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collnotes
{
    /// <summary>
    /// Export project.
    /// </summary>
    public partial class ExportProject : ContentPage
    {
        private List<Project> projectList;
        private Project selectedProject;

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
                    await ExportData();
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
            await ExportData();
        }

        public async Task ExportData()
        {
            var result = await CheckExternalFilePermissions();
            if (!result)
            {
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("Storage permission required for data export!");
            }
            string filePath = DependencyService.Get<ICrossPlatform_GetShareFolder>().GetShareFolder();
            // save to local app data
            // to share in email must use temporary file, can't use internal storage
            string fileName = selectedProject.ProjectName.Trim() + ".csv";
            string localFileLocation = Path.Combine(filePath, fileName);

            var sites = (from s in DataFunctions.GetSitesByProjectName(selectedProject.ProjectName)
                         select s).ToList();

            using (var writer = new StreamWriter(localFileLocation))
            using (var csv = new CsvWriter(writer))
            {
                if (AppVariables.DataExportFormat.Equals(ExportTypes.DarwinCore)) // Darwin Core format
                {
                    csv.WriteHeader<DwCMap>();
                    csv.NextRecord(); // next meme
                                      // write Site
                    foreach (var si in sites)
                    {
                        DwCMap convSite = new DwCMap(si);
                        csv.WriteRecord<DwCMap>(convSite);
                        csv.NextRecord(); // next meme

                        // write Specimen
                        foreach (var sp in DataFunctions.GetSpecimen(si.SiteName))
                        {
                            DwCMap convSpec = new DwCMap(sp);
                            csv.WriteRecord<DwCMap>(convSpec);
                            csv.NextRecord(); // next meme
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                await SendEmail("Project Export", "Set your recipients and send the export data...", new List<string>(), localFileLocation);
            }
            else
            {
                CrossShareFile.Current.ShareLocalFile(localFileLocation, "Share Specimen Export");
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