using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using collNotes.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace collNotes.ViewModels
{
    public class SitesViewModel : BaseViewModel
    {
        public ObservableCollection<Site> Sites { get; set; }
        public Command LoadSitesCommand { get; set; }

        private readonly SiteService siteService =
            DependencyService.Get<SiteService>(DependencyFetchTarget.NewInstance);
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public SitesViewModel()
        {
            Title = "Sites";
            Sites = new ObservableCollection<Site>();

            LoadSitesCommand = new Command(async () => await ExecuteLoadSitesCommand());

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteSites", (sender) =>
            {
                siteService.DeleteAll();
            });
        }

        private async Task ExecuteLoadSitesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Sites.Clear();
                var sites = await siteService.GetAllAsync();
                sites = sites
                    .OrderBy(site => site.AssociatedTripNumber)
                    .ThenBy(site => site.SiteNumber);

                sites.ForEach(site => Sites.Add(site));
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}