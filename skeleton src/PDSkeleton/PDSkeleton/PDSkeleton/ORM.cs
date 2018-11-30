using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

/*
 * Code for ORM SQLite local database.
 * Database for each represented data abstraction - Project, Trip, Site, Specimen
 */

namespace PDSkeleton
{
    // static class - one instance so the connection is available to all other classes and programmer won't have to manage instances
    //              - also lets us not worry about file-access conflicts so much by ALWAYS accessing the SQLite database through this static variable
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

        // no args method
        // returns string with file path for the sqlite database
        // will be different on iOS vs Android
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

        public static bool CheckExists(object obj)
        {
            bool exists = false;

            if (obj.GetType() == typeof(Project))
            {
                Project objProject = (Project)obj;
                ORM.GetConnection().CreateTable<Project>();

                // check for duplicate names first
                List<Project> existingProjects = ORM.GetConnection().Query<Project>("select * from Project");

                foreach (Project p in existingProjects)
                {
                    if (p.ProjectName.Equals(objProject.ProjectName))
                    {
                        exists = true;
                        break;
                    }
                }
            }
            else if (obj.GetType() == typeof(Trip))
            {
                Trip objTrip = (Trip)obj;
                ORM.GetConnection().CreateTable<Trip>();

                // check for duplicate names first
                List<Trip> existingTrips = ORM.GetConnection().Query<Trip>("select * from Trip");

                foreach (Trip p in existingTrips)
                {
                    if (p.TripName.Equals(objTrip.TripName))
                    {
                        exists = true;
                        break;
                    }
                }
            }
            else if (obj.GetType() == typeof(Site))
            {
                Site objSite = (Site)obj;
                ORM.GetConnection().CreateTable<Site>();

                // check for duplicate names first
                List<Site> existingSites = ORM.GetConnection().Query<Site>("select * from Site");

                foreach (Site p in existingSites)
                {
                    if (p.SiteName.Equals(objSite.SiteName))
                    {
                        exists = true;
                        break;
                    }
                }
            }
            else if (obj.GetType() == typeof(Specimen))
            {
                Specimen objSpecimen = (Specimen)obj;
                ORM.GetConnection().CreateTable<Specimen>();

                // check for duplicate names first
                List<Specimen> existingSpecimens = ORM.GetConnection().Query<Specimen>("select * from Specimen");

                foreach (Specimen p in existingSpecimens)
                {
                    if (p.SpecimenName.Equals(objSpecimen.SpecimenName))
                    {
                        exists = true;
                        break;
                    }
                }
            }

            return exists;
        }
    }

    /*
     * There is a table type for each data abstraction
     * Each child-data type has a column that will allow data between tables to be connected
     * Project -> Trip
     * Trip -> Site
     * Site -> Specimen
     */ 

    [Table("Project")]
    public class Project
    {
        [PrimaryKey, AutoIncrement, Column("recordNo")]
        public int RecordNo { get; set; }
        [Column("ProjectName")] // project name has unique constraint
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
        [Column("SpecimenName")]
        public string SpecimenName { get; set; }
    }
}