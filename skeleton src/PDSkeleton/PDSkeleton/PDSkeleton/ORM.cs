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
        public static string SqliteFileName = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "Library/PD_Project_Records.db3";

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

                // check for duplicate names first
                List<Project> existingProjects = GetConnection().Query<Project>("select * from Project");

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

                // check for duplicate names first
                List<Trip> existingTrips = GetConnection().Query<Trip>("select * from Trip");

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

                // check for duplicate names first
                List<Site> existingSites = GetConnection().Query<Site>("select * from Site");

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

                // check for duplicate names first
                List<Specimen> existingSpecimens = GetConnection().Query<Specimen>("select * from Specimen");

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

        public static int InsertObject(object obj)
        {
            int result = 0;

            if (obj.GetType() == typeof(Project))
            {
                Project objProject = (Project)obj;
                result = GetConnection().Insert(objProject);
            }
            else if (obj.GetType() == typeof(Trip))
            {
                Trip objTrip = (Trip)obj;
                result = GetConnection().Insert(objTrip);
            }
            else if (obj.GetType() == typeof(Site))
            {
                Site objSite = (Site)obj;
                result = GetConnection().Insert(objSite);
            }
            else if (obj.GetType() == typeof(Specimen))
            {
                Specimen objSpecimen = (Specimen)obj;
                result = GetConnection().Insert(objSpecimen);
            }

            return result;
        }
    
        public static List<Project> GetProjects()
        {
            return GetConnection().Query<Project>("select * from Project");
        }

        public static List<Trip> GetTrips(string ProjectName)
        {
            return GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + ProjectName + "'");
        }

        public static List<Site> GetSites(string TripName)
        {
            return GetConnection().Query<Site>("select * from Site where TripName = '" + TripName + "'");
        }

        public static List<Specimen> GetSpecimen(string SiteName)
        {
            return GetConnection().Query<Specimen>("select * from Specimen where SiteName = '" + SiteName + "'");
        }

        public static int GetAllProjectsCount()
        {
            return GetConnection().Query<Project>("select * from Project").Count;
        }

        public static int GetAllTripsCount()
        {
            return GetConnection().Query<Trip>("select * from Trip").Count;
        }

        public static int GetAllSitesCount()
        {
            return GetConnection().Query<Site>("select * from Site").Count;
        }

        public static Project GetProjectByName(string name)
        {
            List<Project> results = GetConnection().Query<Project>("select * from Project where ProjectName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
        }

        public static Trip GetTripByName(string name)
        {
            List<Trip> results = GetConnection().Query<Trip>("select * from Trip where TripName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
        }

        public static Site GetSiteByName(string name)
        {
            List<Site> results = GetConnection().Query<Site>("select * from Site where SiteName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
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
        [Column("SpecimenNumber")]
        public int SpecimenNumber { get; set; }
    }
}