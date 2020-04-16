﻿using collNotes.Settings;
using collNotes.ViewModels;
using System;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel viewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            var loadingDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetLoadingDialogConfiguration();
            using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Saving settings...",
                configuration: loadingDialogConfig))
            {
                viewModel.LastSavedDateTimeString = viewModel.GetLastSavedDateTimeString(viewModel.GetLastSavedSettingDateTime());
                await viewModel.UpdateSettings();
            }

            var snackbarConfig = await viewModel.xfMaterialColorConfigFactory.GetSnackbarConfiguration();
            await MaterialDialog.Instance.SnackbarAsync(message: "Settings updated.",
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationLong,
                                            configuration: snackbarConfig);
        }

        private async void RequestPermissions_Clicked(object sender, EventArgs e)
        {
            await viewModel.permissionsService.RequestAllPermissionsAsync();
        }

        private async void Reset_Clicked(object sender, EventArgs e)
        {
            var alertDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetAlertDialogConfiguration();

            // prompt user
            var result = await MaterialDialog.Instance.ConfirmAsync("Are you sure you want to reset all of the data and settings? Note: this is an unrecoverable reset so you should consider creating a backup first!",
                "Confirm", "Yes", "No",
                configuration: alertDialogConfig);
            if (!(result is null || result == false))
            {
                var loadingDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetLoadingDialogConfiguration();
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Resetting all data and settings",
                    configuration: loadingDialogConfig))
                {
                    await viewModel.ResetSettings();
                    MessagingCenter.Send<SettingsPage>(this, "DeleteTrips"); // deleted parent causes related children to be deleted
                }

                var snackbarConfig = await viewModel.xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                await MaterialDialog.Instance.SnackbarAsync(message: "Data reset complete.",
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite,
                                            configuration: snackbarConfig);
            }
        }

        private void CollectionCountTextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(e.NewTextValue, out int intOutput))
            {
                if (intOutput >= 0)
                {
                    viewModel.CurrentCollectionCount = intOutput;
                    CollectionCountTextField.HasError = false;
                }
                else
                {
                    CollectionCountTextField.HasError = true;
                    CollectionCountTextField.ErrorText = "Input should be positive";
                }
            }
            else
            {
                CollectionCountTextField.HasError = true;
                CollectionCountTextField.ErrorText = "Input should be numeric";
            }
        }

        private async void ColorTheme_ChoiceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                CollNotesSettings.ColorTheme selectedTheme = CollNotesSettings.GetByThemeName(e.SelectedItem.ToString());
                await viewModel.SaveTheme(selectedTheme);

                var snackbarConfig = await viewModel.xfMaterialColorConfigFactory.GetSnackbarConfiguration();
                await MaterialDialog.Instance.SnackbarAsync(message: $"Set theme to {e.SelectedItem}. Restart app to load theme.",
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite,
                                            configuration: snackbarConfig);
            }
        }
    }
}