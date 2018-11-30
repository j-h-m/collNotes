using System;

using Xamarin.Forms;

namespace PDSkeleton
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
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
            if (!(pickerExportFormat.SelectedIndex == -1))
            {
                AppVariables.DataExportFormat = pickerExportFormat.SelectedItem.ToString();
            }

            AppVarsFile.WriteAppVars();

            DependencyService.Get<ICrossPlatformToast>().ShortAlert("Saved settings");
        }
    }
}
