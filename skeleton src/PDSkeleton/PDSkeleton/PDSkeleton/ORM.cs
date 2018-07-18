using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

// https://developer.xamarin.com/guides/android/data-and-cloud-services/data-access/part-3-using-sqlite-orm/
// will probably want to go ahead and write data for each column name, maybe this will avoid
// inconsistencies between Table definition and Object definition

namespace PDSkeleton
{
    class ORM
    {
        // define db access here
        private SQLiteConnection connection;
        public SQLiteConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = new SQLiteConnection(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "PD-Project");
            }
        }
    }

    // specify column names
    // table sets column datatype as property datatype
    // include column attributes and property for each

    [Table("ProjectRecords")]
    public class ProjectRecords
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("ProjectName")]
        public string ProjectName { get; set; }
    }

    [Table("TripRecords")]
    public class TripRecords
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("PrimaryCollector")]
        public string PrimaryCollector { get; set; }
        [Column("AdditionalCollectors")]
        public string AdditionalCollectors { get; set; }
        [Column("ProjectNumber")]
        public int ProjectNumber { get; set; }
        [Column("CollectionDate")]
        public DateTime CollectionDate { get; set; }
    }

    [Table("SiteRecords")]
    public class SiteRecords
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("GPSCoordinates")]
        public string GPSCoordinates { get; set; } // this probably won't work
        [Column("Locality")]
        public string Locality { get; set; }
        [Column("Habitat")]
        public string Habitat { get; set; }
        [Column("AssociatedTaxa")]
        public string AssociatedTaxa { get; set; }
        [Column("LocationNotes")]
        public string LocationNotes { get; set; }
        [Column("TripNumber")]
        public int TripNumber { get; set; }
    }

    [Table("SpecimenRecords")]
    public class SpecimenRecords
    {
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