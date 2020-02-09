﻿using collNotes.ViewModels;
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
            // ensure all necessary data is recorded
            if (string.IsNullOrEmpty(viewModel.Trip.TripName))
            {
                await MaterialDialog.Instance.AlertAsync("Trip must have a name!");
                return;
            }
            else
            {
                await viewModel.TripService.CreateAsync(viewModel.Trip);

                // if no primary collector name has been set offer to set it to the current one
                if (string.IsNullOrEmpty(settingsViewModel.CurrentCollectorName))
                {
                    if (!string.IsNullOrEmpty(viewModel.Trip.PrimaryCollector))
                    {
                        var result = await MaterialDialog.Instance.ConfirmAsync($"Primary Collector has not been recorded yet, would you like to set {viewModel.Trip.PrimaryCollector} as the primary collector?", "Confirm", "Yes", "No");
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
    }
}
