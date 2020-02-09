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

        private TripService TripService { get; set; }
        public SiteService SiteService { get; set; }
        public ExceptionRecordService ExceptionRecordService { get; set; }
        public GeoLocationService GeoLocationService { get; set; }
        public CameraService CameraService { get; set; }
        public PermissionsService PermissionsService { get; set; }

        /// <summary>
        /// Constructor for a brand new Site.
        /// </summary>
        public NewSiteViewModel()
        {
            TripService = new TripService(Context);
            SiteService = new SiteService(Context);
            ExceptionRecordService = new ExceptionRecordService(Context);
            GeoLocationService = new GeoLocationService();
            CameraService = new CameraService();
            PermissionsService = new PermissionsService(Context);

            int nextSiteNumber = SiteService.GetNextCollectionNumber().Result;
            AssociableTrips = TripService.GetAllAsync().Result;

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
            TripService = new TripService(Context);
            SiteService = new SiteService(Context);

            int nextSiteNumber = SiteService.GetNextCollectionNumber().Result;
            AssociableTrips = TripService.GetAllAsync().Result;

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