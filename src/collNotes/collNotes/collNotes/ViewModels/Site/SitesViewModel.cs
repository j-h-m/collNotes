using collNotes.Data.Models;
using collNotes.Services;
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
        public SiteService siteService;
        public TripService tripService;
        private IExceptionRecordService exceptionRecordService;

        public SitesViewModel()
        {
            siteService = new SiteService(Context);
            tripService = new TripService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);

            Title = "Sites";
            Sites = new ObservableCollection<Site>();

            LoadSitesCommand = new Command(async () => await ExecuteLoadSitesCommand());
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
                sites = sites.OrderBy(site => site.AssociatedTripNumber);

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