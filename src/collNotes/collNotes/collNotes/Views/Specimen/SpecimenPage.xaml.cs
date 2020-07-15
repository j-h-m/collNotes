using collNotes.ColorThemes.ConfigFactory;
using collNotes.Domain.Models;
using collNotes.Services.Data.RecordData;
using collNotes.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class SpecimenPage : ContentPage
    {
        private readonly SpecimenViewModel viewModel;

        private readonly SiteService siteService =
            DependencyService.Get<SiteService>(DependencyFetchTarget.NewInstance);
        private readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory =
            DependencyService.Get<XfMaterialColorConfigFactory>(DependencyFetchTarget.NewInstance);

        public SpecimenPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SpecimenViewModel();
        }

        private async void OnSpecimenSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Specimen specimen))
                return;

            await Navigation.PushAsync(new SpecimenDetailPage(new SpecimenDetailViewModel(specimen)));

            // Manually deselect item.
            SpecimenListView.SelectedItem = null;
        }

        private async void NewSpecimen_Clicked(object sender, EventArgs e)
        {
            var sites = await siteService.GetAllAsync();
            if (sites.Any())
            {
                await Navigation.PushAsync(new NewSpecimenPage(new NewSpecimenViewModel()));
            }
            else
            {
                var alertDialogConfig = await xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync("Need Sites to associate with a new Specimen!",
                    configuration: alertDialogConfig);
            }
        }

        private async void CloneSpecimen_Clicked(object sender, EventArgs e)
        {
            var choices = viewModel.SpecimenCollection.Select(s => s.SpecimenName).ToList();

            var confirmationDialogConfig = await xfMaterialColorConfigFactory.GetConfirmationDialogConfiguration();
            var result = await MaterialDialog.Instance.SelectChoiceAsync(title: "Select a specimen to clone..",
                                                             choices: choices,
                                                             configuration: confirmationDialogConfig);

            if (result != -1)
            {
                Specimen specimenToClone = viewModel.SpecimenCollection.Where(s => s.SpecimenName == choices[result]).FirstOrDefault();
                await Navigation.PushAsync(new NewSpecimenPage(new NewSpecimenViewModel(specimenToClone)));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadSpecimenCommand.Execute(null);

            CloneButton.IsEnabled = viewModel.SpecimenCollection.Count != 0;
        }
    }
}