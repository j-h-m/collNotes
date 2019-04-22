using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace collnotes.Data
{
    /// <summary>
    /// Data functions.
    /// </summary>
    public static class DataFunctions
    {
        /// <summary>
        /// Checks the database for the object with the name passed in.
        /// Valid object types: Project, Trip, Site, or Specimen.
        /// </summary>
        /// <returns><c>true</c>, if exists was checked, <c>false</c> otherwise.</returns>
        /// <param name="obj">Object.</param>
        /// <param name="name">Name.</param>
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

        /// <summary>
        /// Inserts an object into the database.
        /// Valid object types: Project, Trip, Site, or Specimen.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="obj">Object.</param>
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

        /// <summary>
        /// Gets a list of all Projects.
        /// </summary>
        /// <returns>The projects.</returns>
        public static List<Project> GetProjects()
        {
            return DatabaseFile.GetConnection().Query<Project>("select * from Project");
        }

        /// <summary>
        /// Gets a list of Trips by the ProjectName they were created under.
        /// </summary>
        /// <returns>The trips.</returns>
        /// <param name="ProjectName">Project name.</param>
        public static List<Trip> GetTrips(string ProjectName)
        {
            return DatabaseFile.GetConnection().Query<Trip>("select * from Trip where ProjectName = '" + ProjectName + "'");
        }

        /// <summary>
        /// Gets a list of Sites by the TripName they were created under.
        /// </summary>
        /// <returns>The sites.</returns>
        /// <param name="TripName">Trip name.</param>
        public static List<Site> GetSites(string TripName)
        {
            return DatabaseFile.GetConnection().Query<Site>("select * from Site where TripName = '" + TripName + "'");
        }

        /// <summary>
        /// Gets a list of Specimen by SiteName they were collected under.
        /// </summary>
        /// <returns>The specimen.</returns>
        /// <param name="SiteName">Site name.</param>
        public static List<Specimen> GetSpecimen(string SiteName)
        {
            return DatabaseFile.GetConnection().Query<Specimen>("select * from Specimen where SiteName = '" + SiteName + "'");
        }

        /// <summary>
        /// Gets the name of the sites by project.
        /// </summary>
        /// <returns>The sites by project name.</returns>
        /// <param name="ProjectName">Project name.</param>
        public static List<Site> GetSitesByProjectName(string ProjectName)
        {
            var sitesList = (from el in GetTrips(ProjectName)
                           select el.TripName).ToList().SelectMany(GetSites).ToList();
                                      
            return sitesList.Count > 0 ? sitesList : null;
        }

        /// <summary>
        /// Gets the name of the specimen by project.
        /// </summary>
        /// <returns>The specimen by project name.</returns>
        /// <param name="ProjectName">Project name.</param>
        public static List<Specimen> GetSpecimenByProjectName(string ProjectName)
        {
            var specimenList = (from el in GetTrips(ProjectName)
                                select el.TripName).ToList().SelectMany((arg) => GetSites(arg)).ToList().SelectMany((arg) => GetSpecimen(arg.SiteName)).ToList();
            return specimenList.Count > 0 ? specimenList : null;
        }

        /// <summary>
        /// Gets total of all Projects.
        /// </summary>
        /// <returns>The all projects count.</returns>
        public static int GetAllProjectsCount()
        {
            return DatabaseFile.GetConnection().Query<Project>("select * from Project").Count;
        }

        /// <summary>
        /// Gets total of all Trips.
        /// </summary>
        /// <returns>The all trips count.</returns>
        public static int GetAllTripsCount()
        {
            return DatabaseFile.GetConnection().Query<Trip>("select * from Trip").Count;
        }

        /// <summary>
        /// Gets total of all Sites.
        /// </summary>
        /// <returns>The all sites count.</returns>
        public static int GetAllSitesCount()
        {
            return DatabaseFile.GetConnection().Query<Site>("select * from Site").Count;
        }

        /// <summary>
        /// Gets the specimen count.
        /// This reflects the number of specimen in the Specimen table, does not take into account collection count.
        /// </summary>
        /// <returns>The specimen count.</returns>
        public static int GetAllSpecimenCount()
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

        /// <summary>
        /// Gets a Project by name.
        /// </summary>
        /// <returns>The Project by name.</returns>
        /// <param name="name">Name.</param>
        public static Project GetProjectByName(string name)
        {
            List<Project> results = DatabaseFile.GetConnection().Query<Project>("select * from Project where ProjectName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
        }

        /// <summary>
        /// Gets a Trip by name.
        /// </summary>
        /// <returns>The trip by name.</returns>
        /// <param name="name">Name.</param>
        public static Trip GetTripByName(string name)
        {
            List<Trip> results = DatabaseFile.GetConnection().Query<Trip>("select * from Trip where TripName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
        }

        /// <summary>
        /// Gets a Site by name.
        /// </summary>
        /// <returns>The site by name.</returns>
        /// <param name="name">Name.</param>
        public static Site GetSiteByName(string name)
        {
            List<Site> results = DatabaseFile.GetConnection().Query<Site>("select * from Site where SiteName = '" + name + "'");
            return results.Count > 0 ? results[0] : null;
        }
    }
}
