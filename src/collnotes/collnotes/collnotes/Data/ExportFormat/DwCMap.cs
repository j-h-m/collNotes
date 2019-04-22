namespace collnotes.Data.ExportFormat
{
    /// <summary>
    /// 
    /// </summary>
    public class DwCMap
    {
        public string recordNumber{ get; set; }
        public string siteNumber{ get; set; }
        public string specimenNumber{ get; set; }
        public string genericcolumn2{ get; set; }
        public string associatedCollectors{ get; set; }
        public string habitat{ get; set; }
        public string individualCount{ get; set; }
        public string reproductiveCondition{ get; set; }
        public string locality{ get; set; }
        public string locationRemarks{ get; set; }
        public string occurrenceRemarks{ get; set; }
        public string recordedBy{ get; set; }
        public string labelProject{ get; set; } // Label Project
        public string samplingEffort{ get; set; }
        public string substrate{ get; set; }
        public string associatedTaxa{ get; set; }
        public string eventDate{ get; set; }
        public string establishmentMeans{ get; set; }
        public string genericcolumn1{ get; set; }
        public string decimalLatitude{ get; set; }
        public string decimalLongitude{ get; set; }
        public string coordinateUncertaintyInMeters{ get; set; }
        public string minimumElevationInMeters{ get; set; }
        public string scientificName{ get; set; }
        public string scientificNameAuthorship{ get; set; }
        public string country{ get; set; }
        public string stateProvince{ get; set; }
        public string county{ get; set; }
        public string path{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        public DwCMap(Site site)
        {
            recordNumber = site.RecordNo.ToString() + "-#";
            siteNumber = site.RecordNo.ToString();
            specimenNumber = "";
            genericcolumn2 = "";
            associatedCollectors = DataFunctions.GetTripByName(DataFunctions.GetSiteByName(site.SiteName).TripName).AdditionalCollectors;
            habitat = site.Habitat;
            individualCount = "";
            reproductiveCondition = "";
            locality = site.Locality;
            locationRemarks = site.LocationNotes;
            occurrenceRemarks = "";
            recordedBy = DataFunctions.GetProjectByName(
                            DataFunctions.GetTripByName(
                                DataFunctions.GetSiteByName(site.SiteName).
                            TripName).
                         ProjectName).PrimaryCollector;
            labelProject = DataFunctions.GetTripByName(
                                DataFunctions.GetSiteByName(site.SiteName).
                           TripName).ProjectName;
            samplingEffort = DataFunctions.GetTripByName(
                                DataFunctions.GetSiteByName(site.SiteName).
                             TripName).ProjectName + "-" +
                             DataFunctions.GetProjectByName(
                                DataFunctions.GetTripByName(
                                    DataFunctions.GetSiteByName(site.SiteName).
                                TripName).
                             ProjectName).CreatedDate.ToString("yyyy-MM-dd");
            substrate = "";
            associatedTaxa = site.AssociatedTaxa;
            eventDate = DataFunctions.GetTripByName(
                            DataFunctions.GetSiteByName(site.SiteName).TripName).
                                CollectionDate.ToString("yyyy-MM-dd");
            establishmentMeans = "";
            genericcolumn1 = "";
            decimalLatitude = site.GPSCoordinates?.Split(',')[0];
            decimalLongitude = site.GPSCoordinates?.Split(',')[1];
            coordinateUncertaintyInMeters = site.GPSCoordinates?.Split(',')[2];
            minimumElevationInMeters = site.GPSCoordinates?.Split(',')[3];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specimen"></param>
        public DwCMap(Specimen specimen)
        {
            recordNumber = DataFunctions.GetSiteByName(specimen.SiteName).RecordNo.ToString() + "-" +
                            specimen.RecordNo.ToString();
            siteNumber = DataFunctions.GetSiteByName(specimen.SiteName).RecordNo.ToString();
            specimenNumber = "";
            genericcolumn2 = "";
            associatedCollectors = DataFunctions.GetTripByName(DataFunctions.GetSiteByName(specimen.SiteName).TripName).AdditionalCollectors;
            habitat = DataFunctions.GetSiteByName(specimen.SiteName).Habitat;
            individualCount = "";
            reproductiveCondition = "";
            locality = DataFunctions.GetSiteByName(specimen.SiteName).Locality;
            locationRemarks = DataFunctions.GetSiteByName(specimen.SiteName).LocationNotes;
            occurrenceRemarks = "";
            recordedBy = DataFunctions.GetProjectByName(
                            DataFunctions.GetTripByName(
                                DataFunctions.GetSiteByName(DataFunctions.GetSiteByName(specimen.SiteName).SiteName).
                            TripName).
                         ProjectName).PrimaryCollector;
            labelProject = DataFunctions.GetTripByName(
                                DataFunctions.GetSiteByName(DataFunctions.GetSiteByName(specimen.SiteName).SiteName).
                           TripName).ProjectName;
            samplingEffort = DataFunctions.GetTripByName(
                                DataFunctions.GetSiteByName(DataFunctions.GetSiteByName(specimen.SiteName).SiteName).
                             TripName).ProjectName + "-" +
                             DataFunctions.GetProjectByName(
                                DataFunctions.GetTripByName(
                                    DataFunctions.GetSiteByName(DataFunctions.GetSiteByName(specimen.SiteName).SiteName).
                                TripName).
                             ProjectName).CreatedDate.ToString("yyyy-MM-dd");
            substrate = "";
            associatedTaxa = DataFunctions.GetSiteByName(specimen.SiteName).AssociatedTaxa;
            eventDate = DataFunctions.GetTripByName(
                            DataFunctions.GetSiteByName(DataFunctions.GetSiteByName(specimen.SiteName).SiteName).TripName).
                                CollectionDate.ToString("yyyy-MM-dd");
            establishmentMeans = "";
            genericcolumn1 = "";
            decimalLatitude = DataFunctions.GetSiteByName(specimen.SiteName).GPSCoordinates?.Split(',')[0];
            decimalLongitude = DataFunctions.GetSiteByName(specimen.SiteName).GPSCoordinates?.Split(',')[1];
            coordinateUncertaintyInMeters = DataFunctions.GetSiteByName(specimen.SiteName).GPSCoordinates?.Split(',')[2];
            minimumElevationInMeters = DataFunctions.GetSiteByName(specimen.SiteName).GPSCoordinates?.Split(',')[3];
        }
    }
}
