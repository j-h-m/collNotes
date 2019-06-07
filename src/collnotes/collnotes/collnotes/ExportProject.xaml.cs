using collnotes.Data;
using collnotes.Data.ExportFormat;
using collnotes.Interfaces;
using CsvHelper;
using Plugin.ShareFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using collnotes.Plugins;

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
        public async void ExportProject_SelectedIndexChanged(object sender, EventArgs e)
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
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("No specimen available!");
            }
        }

        /// <summary>
        /// Buttons the export project csv clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public async void ExportProjectCSV_Clicked(object sender, EventArgs e)
        {
            try
            {
                await ExportData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                DependencyService.Get<ICrossPlatformToast>().ShortAlert("No specimen available!");
            }
        }

        public async Task ExportData()
        {
            var result = await CheckPermissions.CheckExternalFilePermissions();
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
                if (AppVariables.DataExportFormat is null || // default is darwin core
                    AppVariables.DataExportFormat.Equals("") || // default is darwin core, should be null if not picked, but let's be safe
                    AppVariables.DataExportFormat.Equals(ExportTypes.DarwinCore)) // Darwin Core format
                {
                    csv.Configuration.RegisterClassMap<DwCMap>();
                    csv.WriteHeader<DwC>();
                    csv.NextRecord(); // next meme
                                      // write Site
                    foreach (var si in sites)
                    {
                        DwC convSite = new DwC(si);
                        csv.WriteRecord<DwC>(convSite);
                        csv.NextRecord(); // next meme

                        // write Specimen
                        foreach (var sp in DataFunctions.GetSpecimen(si.SiteName))
                        {
                            DwC convSpec = new DwC(sp);
                            csv.WriteRecord<DwC>(convSpec);
                            csv.NextRecord(); // next meme
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            // run data export function based on the selected type on iOS
            // Android uses the share function
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                if (AppVariables.ExportTypeIOS is null)
                {
                    CrossShareFile.Current.ShareLocalFile(localFileLocation, "Share Specimen Export");
                }
                else
                {
                    if (AppVariables.ExportTypeIOS.Equals("Multi-Option Share"))
                    {
                        CrossShareFile.Current.ShareLocalFile(localFileLocation, "Share Specimen Export");
                    }
                    else if (AppVariables.ExportTypeIOS.Equals("Email"))
                    {
                        await SendEmail.CreateAndSend(selectedProject.ProjectName + " Export", "Set your recipients and send the export data...", new List<string>(), localFileLocation);
                    }
                }
            }
            else
            {
                CrossShareFile.Current.ShareLocalFile(localFileLocation, "Share Specimen Export");
            }
        }
    }
}