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
            viewModel.LastSavedDateTimeString = viewModel.GetLastSavedDateTimeString(viewModel.GetLastSavedSettingDateTime());
            await MaterialDialog.Instance.AlertAsync("Settings saved");
        }

        private async void RequestPermissions_Clicked(object sender, EventArgs e)
        {
            await viewModel.permissionsService.RequestAllPermissionsAsync();
        }

        private async void Reset_Clicked(object sender, EventArgs e)
        {
            // prompt user
            var result = await MaterialDialog.Instance.ConfirmAsync("Are you sure you want to reset all of the data and settings? Note: this is an unrecoverable reset so you should consider creating a backup first!", "Confirm", "Yes", "No");
            if (!(result is null || result == false))
            {
                using (await MaterialDialog.Instance.LoadingDialogAsync(message: "Resetting all data and settings"))
                {
                    viewModel.ResetSettings();
                    MessagingCenter.Send<SettingsPage>(this, "DeleteTrips");
                }

                await MaterialDialog.Instance.SnackbarAsync(message: "Data reset complete.",
                                            actionButtonText: "Ok",
                                            msDuration: MaterialSnackbar.DurationIndefinite);
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
    }
}