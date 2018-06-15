using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PDSkeleton
{
    class ORM
    {
        // define db access here
        private static string databasePath = "";
        private SQLiteConnection connection = null;

        public SQLiteConnection GetConnection()
        {
            if (connection != null)
            {
                return connection;
            }
            else
            {
                connection = new SQLiteConnection(databasePath);
                return connection;
            }
        }
    }

    // define table here
    [Table("RecordsTable")]
    public class DataRecord
    {
        // specify column name
        // table sets column datatype as property datatype
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("tripNumber")]
        public int TripNumber { get; set; }
        [Column("siteNumber")]
        public int SiteNumber { get; set; }
        [Column("specimenNumber")]
        public int SpecimenNumber { get; set; }
        [Column("otherCatalogNumbers")]
        public string OtherCatalogNumbers { get; set; }
        [Column("genericcolumn2")]
        public string GenericColumn2 { get; set; }
        [Column("associatedCollectors")]
        public string AssociatedCollectors { get; set; }
        [Column("habitat")]
        public string Habitat { get; set; }
        [Column("individualCount")]
        public string IndividualCount { get; set; }
        [Column("reproductiveCondition")]
        public string ReproductiveCondition { get; set; }
        [Column("locality")]
        public string Locality { get; set; }
        [Column("locationRemarks")]
        public string LocationRemarks { get; set; }
        [Column("occurrenceRemarks")]
        public string OccurrenceRemarks { get; set; }
        [Column("recordedBy")]
        public string RecordedBy { get; set; }
        [Column("samplingEffort")]
        public string SamplingEffort { get; set; }
        [Column("substrate")]
        public string Substrate { get; set; }
        [Column("associatedTaxa")]
        public string AssociatedTaxa { get; set; }
        [Column("eventDate")]
        public DateTime EventDate { get; set; }
        [Column("establishmentMeans")]
        public string EstablishmentMeans { get; set; }
        [Column("genericcolumn1")]
        public string GenericColumn1 { get; set; }
        [Column("decimalLatitude")]
        public string DecimalLatitude { get; set; }
        [Column("decimalLongitude")]
        public string DecimalLongitude { get; set; }
        [Column("coordinateUncertaintyInMeters")]
        public string CoordinateUncertaintyInMeters { get; set; }
        [Column("minimumElevationInMeters")]
        public string MinimumElevationInMeters { get; set; }
        [Column("scientificName")]
        public string ScientificName { get; set; }
        [Column("scientificNameAuthorship")]
        public string ScientificNameAuthorship { get; set; }
        [Column("country")]
        public string Country { get; set; }
        [Column("stateProvince")]
        public string StateProvince { get; set; }
        [Column("county")]
        public string County { get; set; }
        [Column("path")]
        public string Path { get; set; }
    }
}