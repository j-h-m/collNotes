using collNotes.Data.Models;
using collNotes.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class TripsPage : ContentPage
    {
        private readonly TripsViewModel viewModel;

        public TripsPage()
        {
            BindingContext = viewModel = new TripsViewModel();
            InitializeComponent();
        }

        private async void OnTripSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Trip trip))
                return;

            await Navigation.PushAsync(new TripDetailPage(new TripDetailViewModel(trip)));

            // Manually deselect item.
            TripsListView.SelectedItem = null;
        }

        private async void NewTrip_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTripPage(new NewTripViewModel()));
        }

        private async void CloneTrip_Clicked(object sender, EventArgs e)
        {
            var choices = viewModel.Trips.Select(t => t.TripName).ToList();

            var confirmationDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetConfirmationDialogConfiguration();
            var result = await MaterialDialog.Instance.SelectChoiceAsync(title: "Select a trip to clone..",
                                                                choices: choices, configuration: confirmationDialogConfig);

            if (result != -1)
            {
                Trip tripToClone = viewModel.Trips.Where(t =>
                    t.TripName == choices[result]).
                    FirstOrDefault();

                await Navigation.PushAsync(new NewTripPage(new NewTripViewModel(tripToClone)));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadTripsCommand.Execute(null);

            CloneButton.IsEnabled = viewModel.Trips.Count != 0;
        }
    }
}