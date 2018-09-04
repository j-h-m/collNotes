using System;
using System.IO;
using SQLite;

namespace PDSkeleton
{
    public static class LiteDB
    {
        private static string sqliteFilename = "PD_Project_Records.db3";
#if __ANDROID__
// Just use whatever directory SpecialFolder.Personal returns
private static string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#else
        // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
        // (they don't want non-user-generated data in Documents)
        private static string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
        private static string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder instead
#endif
        private static string path = Path.Combine(libraryPath, sqliteFilename);

        // our single instance connection the the SQLite Database file
        // opened on app start
        public static SQLiteConnection Connection = new SQLiteConnection(path);
    }

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
        [Column("Project")]
        public string Project { get; set; }
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
        [Column("Trip")]
        public string Trip { get; set; }
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
        [Column("Site")]
        public string Site { get; set; }
    }
}