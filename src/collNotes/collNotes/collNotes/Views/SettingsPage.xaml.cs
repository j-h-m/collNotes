using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.Permissions;
using collNotes.Settings;
using collNotes.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel viewModel =
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        private readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory =
            DependencyService.Get<XfMaterialColorConfigFactory>(DependencyFetchTarget.NewInstance);

        private readonly IPermissionsService permissionsService =
            DependencyService.Get<IPermissionsService>(DependencyFetchTarget.NewInstance);

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.SetSettingsToSavedValues().ConfigureAwait(false);
            viewModel.LastSavedDateTimeString = viewModel.GetLastSavedDateTime(viewModel.GetLastSavedSettingDateTime());
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var loadingDialogConfig = await xfMaterialColorConfigFactory.GetLoadingDialogConfiguration();
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Saving settings...",
                configuration: loadingDialogConfig))
            {
                viewModel.LastSavedDateTimeString = viewModel.GetLastSavedDateTime(viewModel.GetLastSavedSettingDateTime());
                await viewModel.UpdateSettings();
            }

            var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
            await MaterialDialog.Instance.SnackbarAsync(message: "Settings updated.",
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationLong,
                                            configuration: snackbarConfig);
        }

        private async void RequestPermissions_Clicked(object sender, EventArgs e)
        {
            await permissionsService.RequestAllPermissionsAsync();
        }

        private async void Reset_Clicked(object sender, EventArgs e)
        {
            var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();

            // prompt user
            var result = await MaterialDialog.Instance.ConfirmAsync("Are you sure you want to reset all of the data and settings? Note: this is an unrecoverable reset so you should consider creating a backup first!",
                "Confirm", "Yes", "No",
                configuration: alertDialogConfig);
            if (!(result is null || result == false))
            {
                var loadingDialogConfig = await xfMaterialColorConfigFactory.GetLoadingDialogConfiguration();
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Resetting all data and settings",
                    configuration: loadingDialogConfig))
                {
                    // await viewModel.ResetSettings();
                    MessagingCenter.Send<SettingsPage>(this, "DeleteTrips");
                    MessagingCenter.Send<SettingsPage>(this, "DeleteSites");
                    MessagingCenter.Send<SettingsPage>(this, "DeleteSpecimen");
                    MessagingCenter.Send<SettingsPage>(this, "DeleteImportRecords");
                    MessagingCenter.Send<SettingsPage>(this, "DeleteSettings");
                    MessagingCenter.Send<SettingsPage>(this, "DeleteExceptionRecords");
                }

                var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                await MaterialDialog.Instance.SnackbarAsync(message: "Data reset complete.",
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite,
                                            configuration: snackbarConfig);
            }
        }

        private async void ColorTheme_ChoiceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                CollNotesSettings.ColorTheme selectedTheme = CollNotesSettings.GetByThemeName(e.SelectedItem.ToString());
                await viewModel.SaveTheme(selectedTheme);

                var snackbarConfig = await xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                await MaterialDialog.Instance.SnackbarAsync(message: $"Set theme to {e.SelectedItem}.",
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite,
                                            configuration: snackbarConfig);
            }
        }
    }
}