using System;
using System.Collections.Generic;

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
        {

        }

        void btnSaveSettings_Clicked(object sender, System.EventArgs e)
        {

        }
    }

    // application settings values
    //  - read from settings file
    //  - save under settings page
    public static class PD_Settings
    {
        public static string CollectorName = "";
        public static int CollectionStartIndex = 0;
        public static string DataExportFormat = "";
    }
}
