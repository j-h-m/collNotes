using System;
using collNotes.Domain.Models;
using collNotes.Services.Data.RecordData;
using collNotes.Settings;

namespace collNotes.CsvHelperMaps
{
    public class DarwinCore
    {
        #region properties

        public string recordNumber { get; set; }
        public string siteNumber { get; set; }
        public string specimenNumber { get; set; }
        public string genericcolumn2 { get; set; }
        public string associatedCollectors { get; set; }
        public string habitat { get; set; }
        public string individualCount { get; set; }
        public string reproductiveCondition { get; set; }
        public string locality { get; set; }
        public string locationRemarks { get; set; }
        public string occurrenceRemarks { get; set; }
        public string recordedBy { get; set; }
        public string labelProject { get; set; } // Label Project
        public string samplingEffort { get; set; }
        public string substrate { get; set; }
        public string associatedTaxa { get; set; }
        public string eventDate { get; set; }
        public string establishmentMeans { get; set; }
        public string genericcolumn1 { get; set; }
        public string decimalLatitude { get; set; }
        public string decimalLongitude { get; set; }
        public string coordinateUncertaintyInMeters { get; set; }
        public string minimumElevationInMeters { get; set; }
        public string scientificName { get; set; }
        public string scientificNameAuthorship { get; set; }
        public string country { get; set; }
        public string stateProvince { get; set; }
        public string county { get; set; }
        public string path { get; set; }
        public string georeferenceProtocol { get; set; } // added and holds device info
        public string photoB64 { get; set; }

        #endregion properties

        #region constructors

        public DarwinCore() { }

        public DarwinCore(Site site, TripService tripService)
        {
            if (site is null)
                throw new ArgumentNullException(nameof(site));
            if (tripService is null)
                throw new ArgumentNullException(nameof(tripService));

            Trip trip = tripService.GetByNameAsync(site.AssociatedTripName).Result;

            recordNumber = site.SiteNumber.ToString() + "-#";
            siteNumber = site.SiteNumber.ToString();
            specimenNumber = "#";
            genericcolumn2 = "";
            associatedCollectors = trip.AdditionalCollectors;
            habitat = site.Habitat;
            individualCount = "";
            reproductiveCondition = "";
            locality = site.Locality;
            locationRemarks = site.LocationNotes;
            occurrenceRemarks = "";
            recordedBy = trip.PrimaryCollector;
            labelProject = trip.TripName;
            samplingEffort = trip.CollectionDate.ToString("yyyy-MM-dd");
            substrate = "";
            associatedTaxa = site.AssociatedTaxa;
            eventDate = trip.CollectionDate.ToString("yyyy-MM-dd");
            establishmentMeans = CollNotesSettings.DeviceInfo;
            genericcolumn1 = "";
            decimalLatitude = site.Latitude;
            decimalLongitude = site.Longitude;
            coordinateUncertaintyInMeters = site.CoordinateUncertaintyInMeters;
            minimumElevationInMeters = site.MinimumElevationInMeters;
            georeferenceProtocol = "";
            photoB64 = site.PhotoAsBase64;
        }

        public DarwinCore(Specimen specimen, TripService tripService, SiteService siteService)
        {
            Site site = siteService.GetByNameAsync(specimen.AssociatedSiteName).Result;
            Trip trip = tripService.GetByNameAsync(site.AssociatedTripName).Result;

            recordNumber = site.SiteNumber.ToString() + "-" + specimen.SpecimenNumber.ToString();
            siteNumber = site.SiteNumber.ToString();
            specimenNumber = specimen.SpecimenNumber.ToString();
            genericcolumn2 = specimen.AdditionalInfo;
            associatedCollectors = trip.AdditionalCollectors;
            habitat = site.Habitat;
            individualCount = specimen.IndividualCount;
            reproductiveCondition = specimen.LifeStage;
            locality = site.Locality;
            locationRemarks = site.LocationNotes;
            occurrenceRemarks = specimen.OccurrenceNotes;
            recordedBy = trip.PrimaryCollector;
            labelProject = trip.TripName;
            samplingEffort = trip.CollectionDate.ToString("yyyy-MM-dd");
            substrate = specimen.Substrate;
            associatedTaxa = site.AssociatedTaxa;
            eventDate = trip.CollectionDate.ToString("yyyy-MM-dd");
            establishmentMeans = (specimen.Cultivated) ? "cultivated" : "";
            genericcolumn1 = specimen.FieldIdentification;
            decimalLatitude = site.Latitude;
            decimalLongitude = site.Longitude;
            coordinateUncertaintyInMeters = site.CoordinateUncertaintyInMeters;
            minimumElevationInMeters = site.MinimumElevationInMeters;
            georeferenceProtocol = CollNotesSettings.DeviceInfo;
            photoB64 = specimen.PhotoAsBase64;
        }

        #endregion constructors
    }
}