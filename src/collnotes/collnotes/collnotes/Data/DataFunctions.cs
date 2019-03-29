using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace collnotes.Data
{
    public static class DataFunctions
    {
        public static bool CheckExists(object obj, string name)
        {
            bool exists = false;

            if (obj.GetType() == typeof(Project))
            {
                Project objProject = (Project)obj;

                List<Project> queryResult = DatabaseFile.GetConnection().Query<Project>("select * from Project where ProjectName = '" + name + "'");

                if (queryResult.Count > 0)
                {
                    exists = true;
                }
            }
            else if (obj.GetType() == typeof(Trip))
            {
                Trip objTrip = (Trip)obj;

                List<Trip> queryResult = DatabaseFile.GetConnection().Query<Trip>("select * from Trip where TripName = '" + name + "'");

                if (queryResult.Count > 0)
                {
                    exists = true;
                }
            }
            else if (obj.GetType() == typeof(Site))
            {
                Site objSite = (Site)obj;

                List<Site> queryResult = DatabaseFile.GetConnection().Query<Site>("select * from Site where SiteName = '" + name + "'");

                if (queryResult.Count > 0)
                {
                    exists = true;
                }
            }
            else if (obj.GetType() == typeof(Specimen))
            {
                Specimen objSpecimen = (Specimen)obj;

                List<Specimen> queryResult = DatabaseFile.GetConnection().Query<Specimen>("select * from Specimen where SpecimenName = '" + name + "'");

                if (queryResult.Count > 0)
                {
                    exists = true;
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
                result = DatabaseFile.GetConnection().Insert(objProject);
            }
            else if (obj.GetType() == typeof(Trip))
            {
                Trip objTrip = (Trip)obj;
                result = DatabaseFile.GetConnection().Insert(objTrip);
            }
            else if (obj.GetType() == typeof(Site))
            {
                Site objSite = (Site)obj;
                result = DatabaseFile.GetConnection().Insert(objSite);
            }
            else if (obj.GetType() == typeof(Specimen))
            {
                Specimen objSpecimen = (Specimen)obj;
                result = DatabaseFile.GetConnection().Insert(objSpecimen);
            }

            return result;
        }

        public static List<Project> GetProjects()
        {
            return DatabaseFile.GetConnection().Query<Project>("select * from Project");
        }

        public static List<Trip> GetTrips(string ProjectName)
        {
            return DatabaseFile.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + ProjectName + "'");
        }

        public static List<Site> GetSites(string TripName)
        {
            return DatabaseFile.GetConnection().Query<Site>("select * from Site where TripName = '" + TripName + "'");
        }

        public static List<Specimen> GetSpecimen(string SiteName)
        {
            return DatabaseFile.GetConnection().Query<Specimen>("select * from Specimen where SiteName = '" + SiteName + "'");
        }

        public static int GetAllProjectsCount()
        {
            return DatabaseFile.GetConnection().Query<Project>("select * from Project").Count;
        }

        public static int GetAllTripsCount()
        {
            return DatabaseFile.GetConnection().Query<Trip>("select * from Trip").Count;
        }

        public static int GetAllSitesCount()
        {
            return DatabaseFile.GetConnection().Query<Site>("select * from Site").Count;
        }

        public static int GetSpecimenCount()
        {
            int count = 0;
            try
            {
                count = DatabaseFile.GetConnection().Query<Specimen>("select * from Specimen").Count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                count = 0;
            }
            return count;
        }

        public static Trip GetTripByName(string name)
        {
            List<Trip> results = DatabaseFile.GetConnection().Query<Trip>("select * from Trip where TripName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
        }

        public static Site GetSiteByName(string name)
        {
            List<Site> results = DatabaseFile.GetConnection().Query<Site>("select * from Site where SiteName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
        }
    }
}
