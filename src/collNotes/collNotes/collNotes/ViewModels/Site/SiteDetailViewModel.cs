using collNotes.Data.Models;
using collNotes.Factories;
using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Settings;
using System.Collections.Generic;

namespace collNotes.ViewModels
{
    public class SiteDetailViewModel : BaseViewModel
    {
        public Site Site { get; set; }
        public IEnumerable<Trip> AssociableTrips { get; set; }
        public string AssociatedTripName { get; set; }

        public readonly SiteService siteService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;

        private readonly TripService tripService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;
        private readonly IExceptionRecordService exceptionRecordService;

        public SiteDetailViewModel(Site site)
        {
            tripService = new TripService(Context);
            siteService = new SiteService(Context);
            settingService = new SettingService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            Site = site;
            Title = Site?.SiteName;
            AssociatedTripName = Site?.AssociatedTripName;

            AssociableTrips = tripService.GetAllAsync().Result;
        }
    }
}