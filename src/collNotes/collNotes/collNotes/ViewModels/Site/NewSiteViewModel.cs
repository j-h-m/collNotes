using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.DeviceServices.Camera;
using collNotes.DeviceServices.Geolocation;
using collNotes.DeviceServices.Permissions;
using collNotes.Domain.Models;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using collNotes.Services;
using System;
using System.Collections.Generic;

namespace collNotes.ViewModels
{
    public class NewSiteViewModel : BaseViewModel
    {
        public Site Site { get; set; }
        public IEnumerable<Trip> AssociableTrips { get; set; }
        public string AssociatedTripName { get; set; }
        public bool IsClone { get; set; }

        private readonly TripService tripService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;

        public readonly SiteService siteService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;
        public readonly IExceptionRecordService exceptionRecordService;
        public readonly IGeoLocationService geoLocationService;
        public readonly ICameraService cameraService;
        public readonly IPermissionsService permissionsService;

        /// <summary>
        /// Constructor for a new Site.
        /// </summary>
        public NewSiteViewModel()
        {
            tripService = new TripService(Context);
            siteService = new SiteService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            permissionsService = new PermissionsService(Context);
            settingService = new SettingService(Context);

            geoLocationService = new GeoLocationService();
            cameraService = new CameraService();

            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            int nextSiteNumber = siteService.GetNextCollectionNumber().Result;
            AssociableTrips = tripService.GetAllAsync().Result;

            Site = new Site()
            {
                SiteNumber = nextSiteNumber,
                SiteName = $"{nextSiteNumber}-#"
            };

            Title = Site.SiteName;
            IsClone = false;
        }

        /// <summary>
        /// Constructor for a Site clone.
        /// </summary>
        /// <param name="siteToClone">The site to clone.</param>
        public NewSiteViewModel(Site siteToClone)
        {
            if (siteToClone is null)
                throw new ArgumentNullException(nameof(siteToClone));

            tripService = new TripService(Context);
            siteService = new SiteService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            permissionsService = new PermissionsService(Context);
            settingService = new SettingService(Context);

            geoLocationService = new GeoLocationService();
            cameraService = new CameraService();

            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            int nextSiteNumber = siteService.GetNextCollectionNumber().Result;
            AssociableTrips = tripService.GetAllAsync().Result;

            Site = new Site()
            {
                SiteNumber = nextSiteNumber,
                SiteName = $"{nextSiteNumber}-#",
                AssociatedTripName = siteToClone.AssociatedTripName,
                AssociatedTaxa = siteToClone.AssociatedTaxa,
                CoordinateUncertaintyInMeters = siteToClone.CoordinateUncertaintyInMeters,
                Habitat = siteToClone.Habitat,
                Latitude = siteToClone.Latitude,
                Locality = siteToClone.Locality,
                LocationNotes = siteToClone.LocationNotes,
                Longitude = siteToClone.Longitude,
                MinimumElevationInMeters = siteToClone.MinimumElevationInMeters
            };

            Title = Site.SiteName;
            IsClone = true;
        }
    }
}