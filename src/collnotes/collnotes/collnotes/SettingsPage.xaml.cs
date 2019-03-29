using System;
using Xamarin.Forms;
using collnotes.Data;
using collnotes.Interfaces;

namespace collnotes
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            entryCollectorName.Text = (AppVariables.CollectorName is null) ? "" : AppVariables.CollectorName;
            entryStartingRecordNumber.Text = (AppVariables.CollectionCount == 0) ? "" : AppVariables.CollectionCount.ToString();
            pickerExportFormat.SelectedItem = (AppVariables.DataExportFormat is null) ? "" : AppVariables.DataExportFormat;
        }

        void pickerExportFormat_SelectedIndexChange(object sender, EventArgs e) { }

        void btnSaveSettings_Clicked(object sender, EventArgs e)
        {
            if (!entryStartingRecordNumber.Text.Equals(""))
            {
                int currentSpecimenCount = DataFunctions.GetSpecimenCount();
                if (currentSpecimenCount > 0)
                {
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

            // automatically go back to main page after save
            Navigation.PopAsync();
        }
    }
}
