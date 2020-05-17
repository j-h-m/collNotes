using collNotes.ViewModels;
using System;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class SpecimenDetailPage : ContentPage
    {
        private readonly SpecimenDetailViewModel viewModel;

        public SpecimenDetailPage(SpecimenDetailViewModel viewModel)
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
            await viewModel.specimenService.UpdateAsync(viewModel.Specimen);
            await Navigation.PopAsync();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var alertDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
            bool result = Convert.ToBoolean(await MaterialDialog.Instance.ConfirmAsync(message: "Are you sure you want to delete this Specimen?",
                                    title: "Confirmation",
                                    confirmingText: "Yes",
                                    dismissiveText: "No",
                                    configuration: alertDialogConfig));
            if (result)
            {
                await viewModel.specimenService.DeleteAsync(viewModel.Specimen);
                await Navigation.PopAsync();
            }
        }
    }
}