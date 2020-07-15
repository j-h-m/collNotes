using collNotes.ColorThemes.ConfigFactory;
using collNotes.Services.Data.RecordData;
using collNotes.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class TripDetailPage : ContentPage
    {
        private readonly TripDetailViewModel viewModel;

        private readonly TripService tripService =
            DependencyService.Get<TripService>(DependencyFetchTarget.NewInstance);
        private readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory =
            DependencyService.Get<XfMaterialColorConfigFactory>(DependencyFetchTarget.NewInstance);

        public TripDetailPage(TripDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            await tripService.UpdateAsync(viewModel.Trip);
            await Navigation.PopAsync();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var childItems = await tripService.GetChildrenAsync(viewModel.Trip);
            string siteNames = string.Join(", ", childItems.Keys.Select(s => s.SiteName).ToArray());
            string specimenNames = string.Join(", ", childItems.Values.SelectMany(listSpecimen => listSpecimen).ToList().Select(s => s.SpecimenName).ToArray());

            string message = $"Are you sure you wish to delete {viewModel.Trip.TripName}?";

            if (!string.IsNullOrEmpty(siteNames) && !string.IsNullOrEmpty(specimenNames))
                message = $"Deleting {viewModel.Trip.TripName} will also delete the following:" +
                    Environment.NewLine +
                    $"Sites: {siteNames}." +
                    Environment.NewLine +
                    $"Specimen: {specimenNames}." +
                    Environment.NewLine +
                    "Are you sure you wish to delete this data?";

            var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
            bool result = Convert.ToBoolean(await MaterialDialog.Instance.ConfirmAsync(message,
                                    title: "Confirmation",
                                    confirmingText: "Yes",
                                    dismissiveText: "No",
                                    configuration: alertDialogConfig));
            if (result)
            {
                var deleteResult = await tripService.DeleteAsync(viewModel.Trip);
                await Navigation.PopAsync();
            }
        }
    }
}