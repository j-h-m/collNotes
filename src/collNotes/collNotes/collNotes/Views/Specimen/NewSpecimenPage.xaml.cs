using collNotes.Data.Models;
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
        private readonly NewSpecimenViewModel viewModel;
        private FieldIDSearchPage FieldIDSearchPage { get; set; }

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
                await MaterialDialog.Instance.AlertAsync("A photo requires a Specimen Name");
                return;
            }
            else
            {
                string photoAsBase64 = await viewModel.CameraService.TakePicture(viewModel.ExceptionRecordService, viewModel.Specimen.SpecimenName);

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
            // ensure all necessary data is recorded
            if (string.IsNullOrEmpty(viewModel.Specimen.SpecimenName))
            {
                await MaterialDialog.Instance.AlertAsync("Error saving specimen");
                return;
            }
            if (string.IsNullOrEmpty(viewModel.AssociatedSiteName))
            {
                await MaterialDialog.Instance.AlertAsync("A Specimen must be associated with a Site!");
                return;
            }

            viewModel.Specimen.AssociatedSiteName = viewModel.AssociatedSiteName;

            viewModel.Specimen.LifeStage = !string.IsNullOrEmpty(viewModel.SelectedLifeStage) ?
                viewModel.SelectedLifeStage :
                string.Empty;

            viewModel.Specimen.FieldIdentification = !string.IsNullOrEmpty(FieldID_TextField.Text) ?
                FieldID_TextField.Text :
                string.Empty;

            viewModel.Specimen.LabelString = !string.IsNullOrEmpty(viewModel.Specimen.FieldIdentification) ?
                viewModel.Specimen.FieldIdentification :
                viewModel.Specimen.SpecimenName;

            await viewModel.SpecimenService.CreateAsync(viewModel.Specimen);
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
            await Navigation.PushAsync(FieldIDSearchPage);
        }

        protected override void OnAppearing()
        {
            FieldID_TextField.Text = FieldIDSearchPage?.SelectedFieldID ?? string.Empty;
        }
    }
}