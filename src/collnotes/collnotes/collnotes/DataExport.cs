using System;
using System.Collections.Generic;

namespace collnotes
{
    public class DataExport
    {
        public enum DataExportType
        {
            DarwinCore
        }

        public static string CreateCSVForExport(Project selectedProject, DataExportType det, List<Trip> selectedProjectTrips, Dictionary<string, List<Specimen>> specimenForSites, Dictionary<string, List<Site>> sitesForTrips)
        {
            // create CSV with site # - specimen # for each specimen record
            // use recordno to get site # and specimen #

            string csvContent = "";

            // call function to generate csv data based on format
            switch (det)
            {
                case DataExportType.DarwinCore:
                    csvContent = GenDarwinCore(selectedProject, selectedProjectTrips, specimenForSites, sitesForTrips);
                    break;
            }

            return csvContent;
        }

        private static string GenDarwinCore(Project selectedProject, List<Trip> selectedProjectTrips, Dictionary<string, List<Specimen>> specimenForSites, Dictionary<string, List<Site>> sitesForTrips)
        {
            string csvContent = "";

            // 27 column header
            csvContent += "siteNumber,specimenNumber,genericcolumn2,associatedCollectors,habitat,individualCount,reproductiveCondition,locality,locationRemarks,occurrenceRemarks,recordedBy," +
                "Label Project,samplingEffort,substrate,associatedTaxa,eventDate,establishmentMeans,genericcolumn1,decimalLatitude,decimalLongitude,coordinateUncertaintyInMeters,minimumElevationInMeters," +
                "scientificName,scientificNameAuthorship,country,stateProvince,county,path" + Environment.NewLine;

            foreach (KeyValuePair<string, List<Specimen>> sitesSpecimen in specimenForSites)
            {
                // get site, trip info using the site name
                string siteName = sitesSpecimen.Key;
                string tripName = "";

                Site refSite = ORM.GetSiteByName(siteName);
                Trip trip = ORM.GetTripByName(refSite.TripName);
                tripName = trip.TripName;

                // project level data
                string recordedBy = selectedProject.PrimaryCollector;
                string samplingEffort = selectedProject.ProjectName + "," + selectedProject.CreatedDate.ToString("yyyy-MM-dd");
                string labelProject = selectedProject.ProjectName;

                // trip level data
                string assColl = trip.AdditionalCollectors;
                DateTime eventDate = trip.CollectionDate;

                // adjust recordedBy for associated collectors
                //recordedBy += "|" + (from foo in trip.AdditionalCollectors.Split(',') where !foo.Equals("") select (foo + "|"));
                string[] assCollArray = trip.AdditionalCollectors.Split(',');
                if (assCollArray.Length > 0)
                {
                    for (int i = 0; i < assCollArray.Length; i++)
                    {
                        if (i == assCollArray.Length - 1 && i != 0) // add last collector
                            recordedBy += assCollArray[i];
                        else if (i == assCollArray.Length - 1 && i == 0) // add first and only collector
                            recordedBy += "|" + assCollArray[i];
                        else if (i == 0) // add first collector
                            recordedBy += "|" + assCollArray[i] + "|";
                        else // add additional collectors
                            recordedBy += assCollArray[i] + "|";
                    }
                }

                // recordNumber --- site # - specimen #
                string recordNumber;

                // site level data
                string habitat = refSite.Habitat;
                string locality = refSite.Locality;
                string locationNotes = refSite.LocationNotes;
                string associatedTaxa = refSite.AssociatedTaxa;
                int siteNumber = refSite.RecordNo;

                // specimen level data
                // foreach (Specimen spec in sitesSpecimen.Value)
                for (int i = -1; i < sitesSpecimen.Value.Count; i++)
                {
                    int specimenNumber;
                    string genericColumn2, individualCount, reproductiveCondition, occurrenceRemarks, substrate, establishmentMeans, genericColumn1, latitude, longitude, coordinateUncertaintyMeters, minimumElevationMeters;
                    if (i == -1)
                    { // add the site # record
                        specimenNumber = -1; // should match a collector's desired collection count*
                        recordNumber = siteNumber.ToString() + "-#";
                        genericColumn2 = "";
                        individualCount = "";
                        reproductiveCondition = "";
                        occurrenceRemarks = "";
                        substrate = "";
                        establishmentMeans = "";
                        genericColumn1 = "";
                        latitude = (!refSite.GPSCoordinates.Equals("")) ? refSite.GPSCoordinates.Split(',')[0] : "";
                        longitude = (!refSite.GPSCoordinates.Equals("")) ? refSite.GPSCoordinates.Split(',')[1] : "";
                        coordinateUncertaintyMeters = (!refSite.GPSCoordinates.Equals("")) ? refSite.GPSCoordinates.Split(',')[2] : "";
                        minimumElevationMeters = (!refSite.GPSCoordinates.Equals("")) ? minimumElevationMeters = refSite.GPSCoordinates.Split(',')[3] : "";
                    }
                    else
                    {
                        Specimen spec = sitesSpecimen.Value[i];
                        specimenNumber = spec.SpecimenNumber; // should match a collector's desired collection count*
                        recordNumber = siteNumber.ToString() + "-" + specimenNumber.ToString();
                        genericColumn2 = spec.AdditionalInfo;
                        individualCount = spec.IndividualCount;
                        reproductiveCondition = spec.LifeStage;
                        occurrenceRemarks = spec.OccurrenceNotes;
                        substrate = spec.Substrate;
                        establishmentMeans = (spec.Cultivated) ? "cultivated" : "";
                        genericColumn1 = spec.FieldIdentification;
                        latitude = (!spec.GPSCoordinates.Equals("")) ? spec.GPSCoordinates.Split(',')[0] : "";
                        longitude = (!spec.GPSCoordinates.Equals("")) ? spec.GPSCoordinates.Split(',')[1] : "";
                        coordinateUncertaintyMeters = (!spec.GPSCoordinates.Equals("")) ? spec.GPSCoordinates.Split(',')[2] : "";
                        minimumElevationMeters = (!spec.GPSCoordinates.Equals("")) ? minimumElevationMeters = spec.GPSCoordinates.Split(',')[3] : "";
                    }

                    csvContent += "\"" + recordNumber + "\",\"" +                                       // record number
                        siteNumber.ToString() + "\",\"" +                                               // site number
                    ((specimenNumber == -1) ? "#" : specimenNumber.ToString()) + "\",\"" +              // specimen number
                                            genericColumn2 + "\",\"" +                                  // generic column 2 (additional info)
                                            assColl + "\",\"" +                                         // associated collectors
                                            habitat + "\",\"" +                                         // habitat
                                            individualCount + "\",\"" +                                 // individual count
                                            reproductiveCondition + "\",\"" +                           // reproductive condition
                                            locality + "\",\"" +                                        // locality
                                            locationNotes + "\",\"" +                                   // location remarks
                                            occurrenceRemarks + "\",\"" +                               // occurrence remarks
                                            recordedBy + "\",\"" +                                      // recorded by (primary collector and all additional collectors)
                                            labelProject + "\",\"" +                                    // Label Project (project name)
                                            samplingEffort + "\",\"" +                                  // sampling effort (project name, created date)
                                            substrate + "\",\"" +                                       // substrate
                                            associatedTaxa + "\",\"" +                                  // associated taxa
                                            eventDate.ToString("yyyy-MM-dd") + "\",\"" +                // event date - ISO Format*
                                            establishmentMeans + "\",\"" +                              // establishment means (cultivated)
                                            genericColumn1 + "\",\"" +                                  // generic column 1 (field identification)
                                            latitude + "\",\"" +                                        // latitude
                                            longitude + "\",\"" +                                       // longitude
                                            coordinateUncertaintyMeters + "\",\"" +                     // error in Meters
                                            minimumElevationMeters +                                    // elevation
                                            "\"," + "," + "," + "," + "," + "," + Environment.NewLine;  // 6 empty columns for desktop determinations
                } // wrap fields in double quotes and test with user input fields including commas and try single quotes
            }

            return csvContent;
        }
    }
}
