using System;

using Xamarin.Forms;

namespace PDSkeleton
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // load existing settings if exist
            entryCollectorName.Text = (AppVariables.CollectorName is null) ? "" : AppVariables.CollectorName;
            entryStartingRecordNumber.Text = (AppVariables.CollectionCount == 0) ? "" : AppVariables.CollectionCount.ToString();
            pickerExportFormat.SelectedItem = (AppVariables.DataExportFormat is null) ? "" : AppVariables.DataExportFormat;
        }

        void pickerExportFormat_SelectedIndexChange(object sender, EventArgs e)
        { }

        void btnSaveSettings_Clicked(object sender, EventArgs e)
        {
            if (!entryStartingRecordNumber.Text.Equals(""))
            {
                AppVariables.CollectionCount = int.Parse(entryStartingRecordNumber.Text);
            }
            if (!entryCollectorName.Text.Equals(""))
            {
                AppVariables.CollectorName = entryCollectorName.Text;
            }
            if (pickerExportFormat.SelectedIndex != -1)
            {
                AppVariables.DataExportFormat = pickerExportFormat.SelectedItem.ToString();
            }

            AppVarsFile.WriteAppVars();

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved settings");
        }
    }
}