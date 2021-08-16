using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.Camera;
using collNotes.Domain.Models;
using collNotes.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class NewSpecimenPage : ContentPage
    {
        public Site SelectedSite { get; set; }

        private FieldIDSearchPage FieldIDSearchPage { get; set; }
        private readonly NewSpecimenViewModel viewModel;

        private readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory =
            DependencyService.Get<XfMaterialColorConfigFactory>(DependencyFetchTarget.NewInstance);
        private readonly ICameraService cameraService =
            DependencyService.Get<ICameraService>(DependencyFetchTarget.NewInstance);

        public NewSpecimenPage(NewSpecimenViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            FieldIDSearchPage = new FieldIDSearchPage();
        }

        /// <summary>
        /// Take a picture of the Specimen, convert to base64, and save the base64 encoding to the Specimen Photo64 property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TakePicture_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Specimen.SpecimenName))
            {
                var alertDialogconfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync("A photo requires a Specimen Name",
                    configuration: alertDialogconfig);
                return;
            }
            else
            {
                string photoAsBase64 = await cameraService.TakePicture(viewModel.Specimen.SpecimenName);

                viewModel.Specimen.PhotoAsBase64 = !string.IsNullOrEmpty(photoAsBase64) ?
                    photoAsBase64 :
                    string.Empty;
            }
        }

        /// <summary>
        /// Save the Specimen and leave the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Save_Clicked(object sender, EventArgs e)
        {
            var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();

            // ensure all necessary data is recorded
            if (string.IsNullOrEmpty(viewModel.Specimen.SpecimenName))
            {
                await MaterialDialog.Instance.AlertAsync("Error saving specimen",
                    configuration: alertDialogConfig);
                return;
            }
            if (string.IsNullOrEmpty(viewModel.AssociatedSiteName))
            {
                await MaterialDialog.Instance.AlertAsync("A Specimen must be associated with a Site!",
                    configuration: alertDialogConfig);
                return;
            }

            viewModel.Specimen.AssociatedSiteName = viewModel.AssociatedSiteName;

            var associatedSite = viewModel.AssociableSites.First(s => s.SiteName == viewModel.AssociatedSiteName);
            viewModel.Specimen.AssociatedSiteNumber = associatedSite.SiteNumber;

            viewModel.Specimen.LifeStage = !string.IsNullOrEmpty(viewModel.SelectedLifeStage) ?
                viewModel.SelectedLifeStage :
                string.Empty;

            viewModel.Specimen.FieldIdentification = !string.IsNullOrEmpty(FieldID_TextField.Text) ?
                FieldID_TextField.Text :
                string.Empty;

            viewModel.Specimen.LabelString = !string.IsNullOrEmpty(viewModel.Specimen.FieldIdentification) ?
                viewModel.Specimen.FieldIdentification :
                viewModel.Specimen.SpecimenName;

            if (await viewModel.Create(viewModel.Specimen))
            {
                await viewModel.UpdateCollectionNumber();
            }
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Cancel creating a new specimen and leave the page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /// <summary>
        /// Update the title when the associated site name changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedSiteName_ChoiceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedSite = viewModel.AssociableSites.First(s =>
                s.SiteName == viewModel.AssociatedSiteName);

            viewModel.Specimen.SpecimenName = $"{SelectedSite.SiteNumber}-{viewModel.Specimen.SpecimenNumber}";

            Title = viewModel.Specimen.SpecimenName;
        }

        /// <summary>
        /// Push FieldIDSearchPage to top of navigation stack for searching sci names
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchFieldIDButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(FieldIDSearchPage);
        }

        protected override void OnAppearing()
        {
            FieldID_TextField.Text = FieldIDSearchPage?.SelectedFieldID ?? string.Empty;
        }
    }
}