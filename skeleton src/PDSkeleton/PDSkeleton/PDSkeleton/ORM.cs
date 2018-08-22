using System;
using System.IO;
using SQLite;

// need to test reading and writing
// writing to final CSV will require some inner joins +_+'
// https://developer.xamarin.com/guides/android/data-and-cloud-services/data-access/part-3-using-sqlite-orm/

namespace PDSkeleton
{
    public static class ORM
    {
        private static SQLiteConnection Connection = null;

        private static void SetConnection()
        {
            string filePath = CreateDBFilePath();
            Connection = new SQLiteConnection(filePath);
        }

        public static SQLiteConnection GetConnection()
        {
            if (Connection != null)
                return Connection;
            else
            {
                SetConnection();
                return Connection;
            }
            
        }

        private static string CreateDBFilePath()
        {
            var sqliteFilename = "PD_Project_Records.db3";
#if __ANDROID__
// Just use whatever directory SpecialFolder.Personal returns
string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
            // (they don't want non-user-generated data in Documents)
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder instead
#endif
            var path = Path.Combine(libraryPath, sqliteFilename);
            return path;
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
        [Column("PrimaryCollector")]
        public string PrimaryCollector { get; set; }
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Column("CompletedDate")]
        public DateTime CompletedDate { get; set; }
    }

    [Table("TripRecords")]
    public class TripRecords
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("AdditionalCollectors")]
        public string AdditionalCollectors { get; set; }
        [Column("CollectionDate")]
        public DateTime CollectionDate { get; set; }
        [Column("ParentProject")]
        public string ParentProject { get; set; }
    }

    [Table("SiteRecords")]
    public class SiteRecords
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("GPSCoordinates")]
        public string GPSCoordinates { get; set; }
        [Column("Locality")]
        public string Locality { get; set; }
        [Column("Habitat")]
        public string Habitat { get; set; }
        [Column("AssociatedTaxa")]
        public string AssociatedTaxa { get; set; }
        [Column("LocationNotes")]
        public string LocationNotes { get; set; }
        [Column("ParentTrip")]
        public string ParentTrip { get; set; }
    }

    [Table("SpecimenRecords")]
    public class SpecimenRecords
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("ParentSite")]
        public string ParentSite { get; set; }
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