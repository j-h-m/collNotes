using collNotes.ViewModels;
using System;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class NewTripPage : ContentPage
    {
        private readonly NewTripViewModel viewModel;
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public NewTripPage(NewTripViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        /// <summary>
        /// Save Trip and leave page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Clicked(object sender, EventArgs e)
        {
            var alertDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetAlertDialogConfiguration();

            // ensure all necessary data is recorded
            if (string.IsNullOrEmpty(viewModel.Trip.TripName))
            {
                await MaterialDialog.Instance.AlertAsync("Trip must have a name!",
                    configuration: alertDialogConfig);
                return;
            }
            else
            {
                await viewModel.tripService.CreateAsync(viewModel.Trip);

                // if no primary collector name has been set offer to set it to the current one
                if (string.IsNullOrEmpty(settingsViewModel.CurrentCollectorName))
                {
                    if (!string.IsNullOrEmpty(viewModel.Trip.PrimaryCollector))
                    {
                        var result = await MaterialDialog.Instance.ConfirmAsync(GetPrimaryCollectorPrompt(viewModel.Trip.PrimaryCollector), 
                            "Confirm", "Yes", "No",
                            configuration: alertDialogConfig);
                        if (!(result is null || result == false))
                        {
                            settingsViewModel.CurrentCollectorName = viewModel.Trip.PrimaryCollector;
                        }
                    }
                }

                await Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Cancel creating a new Trip and leave page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private string GetPrimaryCollectorPrompt(string primaryCollector)
        {
            return $"Primary Collector has not been set yet, would you like to set [{primaryCollector}] as the primary collector?";
        }
    }
}
