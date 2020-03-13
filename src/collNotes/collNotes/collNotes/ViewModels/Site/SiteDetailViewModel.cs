using collNotes.Data.Models;
using collNotes.Services;

namespace collNotes.ViewModels
{
    public class SiteDetailViewModel : BaseViewModel
    {
        public Site Site { get; set; }
        public SiteService siteService;

        public SiteDetailViewModel(Site site)
        {
            siteService = new SiteService(Context);

            Site = site;
            Title = Site?.SiteName;
        }
    }
}