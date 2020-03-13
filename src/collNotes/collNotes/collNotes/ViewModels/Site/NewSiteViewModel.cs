using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Services.Permissions;
using System.Collections.Generic;

namespace collNotes.ViewModels
{
    public class NewSiteViewModel : BaseViewModel
    {
        public Site Site { get; set; }
        public IEnumerable<Trip> AssociableTrips { get; set; }
        public string AssociatedTripName { get; set; }
        public bool IsClone { get; set; }

        private TripService tripService;
        public SiteService siteService;
        public IExceptionRecordService exceptionRecordService;
        public IGeoLocationService geoLocationService;
        public ICameraService cameraService;
        public IPermissionsService permissionsService;

        /// <summary>
        /// Constructor for a brand new Site.
        /// </summary>
        public NewSiteViewModel()
        {
            tripService = new TripService(Context);
            siteService = new SiteService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            geoLocationService = new GeoLocationService();
            cameraService = new CameraService();
            permissionsService = new PermissionsService(Context);

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
            tripService = new TripService(Context);
            siteService = new SiteService(Context);

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