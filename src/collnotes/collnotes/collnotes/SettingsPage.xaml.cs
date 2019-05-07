using System;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;
using Xamarin.Essentials;

namespace collnotes
{
    /// <summary>
    /// Settings page.
    /// </summary>
    public partial class SettingsPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:collnotes.SettingsPage"/> class.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            entryCollectorName.Text = (AppVariables.CollectorName is null) ? "" : AppVariables.CollectorName;
            entryStartingRecordNumber.Text = (AppVariables.CollectionCount == 0) ? "" : AppVariables.CollectionCount.ToString();
            pickerExportFormat.SelectedItem = (AppVariables.DataExportFormat is null) ? "" : AppVariables.DataExportFormat;

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                LblExportType.IsVisible = true;
                ExportTypeIOS.IsVisible = true;
                ExportTypeIOS.SelectedItem = (AppVariables.ExportTypeIOS is null) ? "" : AppVariables.ExportTypeIOS;
            }
            else // Android
            {
                LblExportType.IsVisible = false;
                ExportTypeIOS.IsVisible = false;
            }
        }

        /// <summary>
        /// Pickers the export format selected index change.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ExportFormat_SelectedIndexChange(object sender, EventArgs e) { }

        /// <summary>
        /// Exports the type ios selected index change.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void ExportTypeIOS_SelectedIndexChange(object sender, EventArgs e) { }

        /// <summary>
        /// Buttons the save settings clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void SaveSettings_Clicked(object sender, EventArgs e)
        {
            if (!entryStartingRecordNumber.Text.Equals(""))
            {
                int currentSpecimenCount = DataFunctions.GetAllSpecimenCount();
                int userEntered = int.Parse(entryStartingRecordNumber.Text);
                if (userEntered < currentSpecimenCount) // the user should not be able to set the collection count to a value lower than the current total
                {
                    DependencyService.Get<ICrossPlatformToast>().ShortAlert("You cannot set the Specimen Count lower than the current total.");
                    return;
                }
                else
                {
                    AppVariables.CollectionCount = userEntered;
                }
            }
            if (!entryCollectorName.Text.Equals(""))
            {
                AppVariables.CollectorName = entryCollectorName.Text;
            }
            if (pickerExportFormat.SelectedIndex != -1)
            {
                AppVariables.DataExportFormat = pickerExportFormat.SelectedItem.ToString();
            }
            if (ExportTypeIOS.SelectedIndex != -1)
            {
                AppVariables.ExportTypeIOS = ExportTypeIOS.SelectedItem.ToString();
            }

            AppVarsFile.WriteAppVars();

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved settings");
        }
    }
}
