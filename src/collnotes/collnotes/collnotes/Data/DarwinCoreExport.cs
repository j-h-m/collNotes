//
//  DarwinCoreExport.cs
//
//  Author:
//       Jacob Motley <programmingisfunjacmot@gmail.com>
//
//  Copyright (c) 2019 
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using collnotes.Interfaces;

namespace collnotes.Data
{
    public class DarwinCoreExport : IExportFormat
    {
        public DarwinCoreExport() { }

        public string GenerateExportText(Project project)
        {
            string exportText = "";
            Dictionary<int, string> columnNames = GetColumnNames();
            Dictionary<int, KeyValuePair<Type, string>> headerMap = GetCSVHeaderMap();

            var siteList = (from s in DataFunctions.GetSitesByProjectName(project.ProjectName)
                            select s).ToList();
            foreach (var site in siteList)
            {
                var specimenList = (from s in DataFunctions.GetSpecimen(site.SiteName)
                                    select s).ToList();

                // site record
                for (int i = 0; i < headerMap.Count; i++)
                {
                    var map = headerMap[i];

                    switch (map.Key.ToString())
                    {
                        case "Project":
                            switch (map.Value)
                            {
                                default:
                                    break;
                            }
                            break;
                        case "Trip":
                            break;
                        case "Site":
                            break;
                        case "Specimen":
                            break;
                        default:
                            break;
                    }
                }

                // specimen records
                foreach (var specimen in specimenList)
                {
                    for (int i = 0; i < headerMap.Count; i++)
                    {
                        var map = headerMap[i];

                        switch (map.Key.ToString())
                        {
                            case "Project":
                                break;
                            case "Trip":
                                break;
                            case "Site":
                                break;
                            case "Specimen":
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return exportText;
        }

        public Dictionary<int, string> GetColumnNames()
        {
            Dictionary<int, string> columnNames = new Dictionary<int, string>
            {
                { 0, "recordNumber" },
                { 1, "siteNumber" },
                { 2, "specimenNumber" },
                { 3, "genericcolumn2" },
                { 4, "associatedCollectors" },
                { 5, "habitat" },
                { 6, "individualCount" },
                { 7, "reproductiveCondition" },
                { 8, "locality" },
                { 9, "locationRemarks" },
                { 10, "occurrenceRemarks" },
                { 11, "recordedBy" },
                { 12, "Label Project" },
                { 13, "samplingEffort" },
                { 14, "substrate" },
                { 15, "associatedTaxa" },
                { 16, "eventDate" },
                { 17, "establishmentMeans" },
                { 18, "genericcolumn1" },
                { 19, "decimalLatitude" },
                { 20, "decimalLongitude" },
                { 21, "coordinateUncertaintyInMeters" },
                { 22, "minimumElevationInMeters" },
                { 23, "scientificName" },
                { 24, "scientificNameAuthorship" },
                { 25, "country" },
                { 26, "stateProvince" },
                { 27, "county" },
                { 28, "path" }
            };

            return columnNames;
        }

        public Dictionary<int, KeyValuePair<Type, string>> GetCSVHeaderMap()
        {
            Dictionary<int, KeyValuePair<Type, string>> headerMap = new Dictionary<int, KeyValuePair<Type, string>>
            {
                {0, new KeyValuePair<Type, string>(typeof(Site), "custom") }, // recordNumber
                {1, new KeyValuePair<Type, string>(typeof(Site), "RecordNo") }, // siteNumber
                {2, new KeyValuePair<Type, string>(typeof(Specimen), "SpecimenNumber") }, // specimenNumber
                {3, new KeyValuePair<Type, string>(typeof(Specimen), "AdditionalInfo") }, // genericcolumn2
                {4, new KeyValuePair<Type, string>(typeof(Trip), "AdditionalCollectors") }, // associatedCollectors
                {5, new KeyValuePair<Type, string>(typeof(Site), "Habitat") }, // habitat
                {6, new KeyValuePair<Type, string>(typeof(Specimen), "IndividualCount") }, // individualCount
                {7, new KeyValuePair<Type, string>(typeof(Specimen), "LifeStage") }, // reproductiveCondition
                {8, new KeyValuePair<Type, string>(typeof(Site), "Locality") }, // locality
                {9, new KeyValuePair<Type, string>(typeof(Site), "LocationNotes") }, // locationRemarks
                {10, new KeyValuePair<Type, string>(typeof(Specimen), "OccurrenceNotes") }, // occurrenceRemarks
                {11, new KeyValuePair<Type, string>(typeof(Project), "PrimaryCollector") }, // recordedBy
                {12, new KeyValuePair<Type, string>(typeof(Project), "ProjectName") }, // Label Project
                {13, new KeyValuePair<Type, string>(typeof(Project), "custom") }, // samplingEffort
                {14, new KeyValuePair<Type, string>(typeof(Specimen), "Substrate") }, // substrate
                {15, new KeyValuePair<Type, string>(typeof(Site), "AssociatedTaxa") }, // associatedTaxa
                {16, new KeyValuePair<Type, string>(typeof(Trip), "CollectionDate") }, // eventDate
                {17, new KeyValuePair<Type, string>(typeof(Specimen), "Cultivated") }, // establishmentMeans
                {18, new KeyValuePair<Type, string>(typeof(Specimen), "FieldIdentification") }, // genericcolumn1
                {19, new KeyValuePair<Type, string>(typeof(Site), "GPSCoordinates0") }, // decimalLatitude
                {20, new KeyValuePair<Type, string>(typeof(Site), "GPSCoordinates1") }, // decimalLongitude
                {21, new KeyValuePair<Type, string>(typeof(Site), "GPSCoordinates2") }, // coordinateUncertaintyInMeters
                {22, new KeyValuePair<Type, string>(typeof(Site), "GPSCoordinates3") }, // minimumElevationInMeters
                {23, new KeyValuePair<Type, string>(typeof(Object), "empty") }, // scientificName
                {24, new KeyValuePair<Type, string>(typeof(Object), "empty") }, // scientificNameAuthorship
                {25, new KeyValuePair<Type, string>(typeof(Object), "empty") }, // country
                {26, new KeyValuePair<Type, string>(typeof(Object), "empty") }, // stateProvince
                {27, new KeyValuePair<Type, string>(typeof(Object), "empty") }, // county
                {28, new KeyValuePair<Type, string>(typeof(Object), "empty") } // path
            };

            return headerMap;
        }

        public bool VerifyExportText(string exportText)
        {
            bool result = false;
            string[] splitText = exportText.Split(',');
            foreach (string el in splitText)
            {

            }
            return result;
        }
    }
}
