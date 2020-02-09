using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Views;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class SiteDetailViewModel : BaseViewModel
    {
        public Site Site { get; set; }
        public SiteService SiteService { get; set; }

        public SiteDetailViewModel(Site site)
        {
            SiteService = new SiteService(Context);

            Site = site;
            Title = Site?.SiteName;
        }
    }
}