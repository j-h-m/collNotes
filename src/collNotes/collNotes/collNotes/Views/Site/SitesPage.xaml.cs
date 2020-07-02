using collNotes.Domain.Models;
using collNotes.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.Views
{
    public partial class SitesPage : ContentPage
    {
        private readonly SitesViewModel viewModel;
        public SitesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SitesViewModel();
        }

        private async void OnSiteSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Site site))
                return;

            await Navigation.PushAsync(new SiteDetailPage(new SiteDetailViewModel(site)));

            // Manually deselect item.
            SitesListView.SelectedItem = null;
        }

        private async void NewSite_Clicked(object sender, EventArgs e)
        {
            var trips = await viewModel.tripService.GetAllAsync();
            if (trips.Any())
            {
                await Navigation.PushAsync(new NewSitePage(new NewSiteViewModel()));
            }
            else
            {
                var alertDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetAlertDialogConfiguration();
                await MaterialDialog.Instance.AlertAsync("Need Trips to associate with a new Site!",
                    configuration: alertDialogConfig);
            }
        }

        private async void CloneSite_Clicked(object sender, EventArgs e)
        {
            var choices = viewModel.Sites.Select(s => s.SiteName).ToList();

            var confirmationDialogConfig = await viewModel.xfMaterialColorConfigFactory.GetConfirmationDialogConfiguration();
            var result = await MaterialDialog.Instance.SelectChoiceAsync(title: "Select a site to clone..",
                                                             choices: choices, configuration: confirmationDialogConfig);

            if (result != -1)
            {
                Site siteToClone = viewModel.Sites.Where(s =>
                    s.SiteName == choices[result]).
                    FirstOrDefault();
                await Navigation.PushAsync(new NewSitePage(new NewSiteViewModel(siteToClone)));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadSitesCommand.Execute(null);

            CloneButton.IsEnabled = viewModel.Sites.Count != 0;
        }
    }
}