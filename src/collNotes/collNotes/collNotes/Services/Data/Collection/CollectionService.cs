﻿using collNotes.CsvHelperMaps;
using collNotes.Domain.Models;
using collNotes.Ef.Context;
using collNotes.Services.Data.RecordData;
using collNotes.ViewModels;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace collNotes.Services.Data
{
    public class CollectionService : ICollectionService
    {
        private readonly TripService tripService =
            DependencyService.Get<TripService>(DependencyFetchTarget.NewInstance);
        private readonly SiteService siteService =
            DependencyService.Get<SiteService>(DependencyFetchTarget.NewInstance);
        private readonly SpecimenService specimenService =
            DependencyService.Get<SpecimenService>(DependencyFetchTarget.NewInstance);
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);
        private readonly SettingsViewModel settingsViewModel = 
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public CollectionService() { }

        public async Task<bool> ExportCollectionData(Trip trip, string exportPath)
        {
            bool result = false;

            try
            {
                using (var streamWriter = new StreamWriter(exportPath))
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture))
                {
                    csvWriter.Configuration.RegisterClassMap<DarwinCoreMap>();
                    csvWriter.WriteHeader<DarwinCore>();
                    csvWriter.NextRecord();

                    var sites = await siteService.GetAllAsync();
                    sites = sites.Where(s => s.AssociatedTripName == trip.TripName).ToList();

                    // write Site
                    foreach (var si in sites)
                    {
                        DarwinCore convSite = new DarwinCore(si, tripService);
                        csvWriter.WriteRecord<DarwinCore>(convSite);
                        csvWriter.NextRecord();

                        var specimen = await specimenService.GetAllAsync();
                        specimen = specimen.Where(s => s.AssociatedSiteName == si.SiteName);

                        // write Specimen
                        foreach (var sp in specimen)
                        {
                            DarwinCore convSpec = new DarwinCore(sp, tripService, siteService);
                            csvWriter.WriteRecord<DarwinCore>(convSpec);
                            csvWriter.NextRecord();
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return result;
        }

        public async Task<bool> ImportCollectionData(Stream stream)
        {
            bool result = false;

            try
            {
                using (var reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    csv.Configuration.RegisterClassMap<DarwinCoreMap>();

                    var records = csv.GetRecords<DarwinCore>().ToList();

                    bool conversionResult = await ImportDataFromDwcList(records);
                    if (!conversionResult)
                        throw new Exception("ImportDataFromDwcList failed to convert records and add to objs database.");

                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return result;
        }

        private async Task<bool> ImportDataFromDwcList(List<DarwinCore> darwinCoreRecords)
        {
            var result = false;
            List<Site> sites = new List<Site>();
            List<Specimen> specimen = new List<Specimen>();

            try
            {
                var randomSiteRecord = darwinCoreRecords.FirstOrDefault(dwcr => dwcr.specimenNumber.Equals("#"));
                if (randomSiteRecord is null)
                    throw new Exception("No Site found in records, invalid format.");

                int nextTripNumber = await tripService.GetNextCollectionNumber();
                Trip importedTrip = new Trip()
                {
                    TripName = $"Trip-{nextTripNumber}",
                    TripNumber = nextTripNumber,
                    AdditionalCollectors = randomSiteRecord.associatedCollectors,
                    CollectionDate = Convert.ToDateTime(randomSiteRecord.eventDate),
                    PrimaryCollector = randomSiteRecord.recordedBy
                };

                await tripService.CreateAsync(importedTrip);

                int nextSiteNumber = await siteService.GetNextCollectionNumber();
                int nextSpecimenNumber = await specimenService.GetNextCollectionNumber(settingsViewModel.CurrentCollectionCount);

                int siteNumber = 0;
                int specimenNumber = 0;

                foreach (var record in darwinCoreRecords)
                {
                    if (record.specimenNumber.Equals("#")) // Site Record
                    {
                        siteNumber = nextSiteNumber + sites.Count == 0 ?
                            1 : nextSiteNumber + sites.Count;

                        sites.Add(new Site()
                        {
                            AssociatedTaxa = record.associatedTaxa,
                            AssociatedTripName = importedTrip.TripName,
                            AssociatedTripNumber = importedTrip.TripNumber,
                            CoordinateUncertaintyInMeters = record.coordinateUncertaintyInMeters,
                            Habitat = record.habitat,
                            Latitude = record.decimalLatitude,
                            Longitude = record.decimalLongitude,
                            Locality = record.locality,
                            LocationNotes = record.locationRemarks,
                            MinimumElevationInMeters = record.minimumElevationInMeters,
                            PhotoAsBase64 = record.photoB64,
                            SiteNumber = siteNumber,
                            SiteName = $"{siteNumber}-#"
                        });
                    }
                    else // Specimen Record
                    {
                        specimenNumber = nextSpecimenNumber + specimen.Count == 0 ?
                            1 : nextSpecimenNumber + specimen.Count;

                        specimen.Add(new Specimen()
                        {
                            AdditionalInfo = record.genericcolumn2,
                            AssociatedSiteName = $"{siteNumber}-#",
                            AssociatedSiteNumber = siteNumber,
                            Cultivated = record.establishmentMeans.Equals("cultivated"),
                            FieldIdentification = record.genericcolumn1,
                            IndividualCount = record.individualCount,
                            LabelString = string.Empty,
                            LifeStage = record.reproductiveCondition,
                            OccurrenceNotes = record.occurrenceRemarks,
                            PhotoAsBase64 = record.photoB64,
                            SpecimenNumber = specimenNumber,
                            SpecimenName = $"{siteNumber}-{specimenNumber}",
                            Substrate = record.substrate
                        });
                    }
                }

                if (sites.Count > 0 && specimen.Count > 0)
                {
                    if (await siteService.CreateMultipleAsync(sites) == false)
                        throw new Exception("siteService failed to create sites created from import in database");

                    if (await specimenService.CreateMultipleAsync(specimen) == false)
                        throw new Exception("specimenService failed to create specimen created from import in database");
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return result;
        }
    }
}