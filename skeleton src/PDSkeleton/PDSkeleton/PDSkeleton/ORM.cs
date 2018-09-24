using System;
using System.IO;
using SQLite;

/*
 * Code for ORM style SQLite local database.
 * Each object will have it's own table.
 * Each object should have some way to trace to it's parent, in terms of data heirarchy.
 * 
 */ 

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

    /*
     * There is a table type for each collected-data type
     * Each child-data type has a column that will allow data between tables to be connected through SQL join query
     */ 

    [Table("Project")]
    public class Project
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

    [Table("Trip")]
    public class Trip
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("AdditionalCollectors")]
        public string AdditionalCollectors { get; set; }
        [Column("CollectionDate")]
        public DateTime CollectionDate { get; set; }
        [Column("ProjectName")]
        public string ProjectName { get; set; }
        [Column("TripName")]
        public string TripName { get; set; }
    }

    [Table("Site")]
    public class Site
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
        [Column("TripName")]
        public string TripName { get; set; }
        [Column("SiteName")]
        public string SiteName { get; set; }
    }

    [Table("Specimen")]
    public class Specimen
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("GPSCoordinates")]
        public string GPSCoordinates { get; set; }
        [Column("FieldIdentification")]
        public string FieldIdentification { get; set; }
        [Column("OccurrenceNotes")]
        public string OccurrenceNotes { get; set; }
        [Column("Substrate")]
        public string Substrate { get; set; }
        [Column("LifeStage")]
        public string LifeStage { get; set; }
        [Column("AdditionalInfo")]
        public string AdditionalInfo { get; set; }
        [Column("IndividualCount")]
        public string IndividualCount { get; set; }
        [Column("Cultivated")]
        public bool Cultivated { get; set; }
        [Column("SiteName")]
        public string SiteName { get; set; }
    }
}