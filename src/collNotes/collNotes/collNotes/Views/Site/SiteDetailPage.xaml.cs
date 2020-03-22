using System;
using System.Linq;
using collNotes.ViewModels;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class SiteDetailPage : ContentPage
    {
        private readonly SiteDetailViewModel viewModel;

        public SiteDetailPage(SiteDetailViewModel viewModel)
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
            await viewModel.siteService.UpdateAsync(viewModel.Site);
            await Navigation.PopAsync();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var childItems = await viewModel.siteService.GetChildrenAsync(viewModel.Site);
            string specimenNames = string.Join(", ", childItems.Select(s => s.SpecimenName).ToArray());

            string message = "Are you sure you want to delete this Site? This will delete all associated Specimen.";

            if (!string.IsNullOrEmpty(specimenNames))
                message = $"Deleting {viewModel.Site.SiteName} will also delete the following:" +
                    Environment.NewLine +
                    $"Specimen: {specimenNames}.";

            bool result = Convert.ToBoolean(await MaterialDialog.Instance.ConfirmAsync(message,
                                    title: "Confirm",
                                    confirmingText: "Yes",
                                    dismissiveText: "No"));
            if (result)
            {
                await viewModel.siteService.DeleteAsync(viewModel.Site);
                await Navigation.PopAsync();
            }
        }
    }
}