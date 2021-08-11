using collNotes.Domain.Models;
using collNotes.Services.Data.RecordData;
using collNotes.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace collNotes.Services.Data
{
    public class BackupService : IBackupService
    {
        private readonly TripService tripService = 
            DependencyService.Get<TripService>(DependencyFetchTarget.NewInstance);
        private readonly SiteService siteService =
            DependencyService.Get<SiteService>(DependencyFetchTarget.NewInstance);
        private readonly SpecimenService specimenService =
            DependencyService.Get<SpecimenService>(DependencyFetchTarget.NewInstance);
        private readonly ISettingService settingService =
            DependencyService.Get<ISettingService>(DependencyFetchTarget.NewInstance);
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);
        private readonly SettingsViewModel settingsViewModel =
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public BackupService() { }

        public async Task<bool> ExportBackup(string exportFilePath)
        {
            bool result = false;

            try
            {
                var trips = await tripService.GetAllAsync();
                var sites = await siteService.GetAllAsync();
                var specimen = await specimenService.GetAllAsync();
                var settings = await settingService.GetAllAsync();
                var exceptionRecords = await exceptionRecordService.GetAllAsync();

                Backup backup = new Backup()
                {
                    Trips = trips.ToList(),
                    Sites = sites.ToList(),
                    Specimen = specimen.ToList(),
                    Settings = settings.ToList(),
                    ExceptionRecords = exceptionRecords.ToList()
                };

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Backup));

                FileStream fileStream = File.Create(exportFilePath);

                xmlSerializer.Serialize(fileStream, backup);

                result = true;
                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                result = false;
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return result;
        }

        public async Task<bool> ImportBackup(Stream stream)
        {
            bool result = false;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Backup));
                StreamReader streamReader = new StreamReader(stream);

                // read into Backup object
                Backup backup = (Backup)xmlSerializer.Deserialize(streamReader);

                // write all objects to database
                backup.Trips.ForEach(async t =>
                {
                    int nextTripNumber = await tripService.GetNextCollectionNumber();
                    string nextTripName = $"Trip-{nextTripNumber}";

                    await tripService.CreateAsync(new Trip()
                    {
                        AdditionalCollectors = t.AdditionalCollectors,
                        CollectionDate = t.CollectionDate,
                        PrimaryCollector = t.PrimaryCollector,
                        TripName = nextTripName,
                        TripNumber = nextTripNumber
                    });

                    // match on saved number
                    // create object with generated number
                    backup.Sites.Where(x => x.AssociatedTripNumber == t.TripNumber).ForEach(async s =>
                    {
                        int nextSiteNumber = await siteService.GetNextCollectionNumber();
                        string nextSiteName = $"{nextSiteNumber}-#";

                        await siteService.CreateAsync(new Site()
                        {
                            AssociatedTaxa = s.AssociatedTaxa,
                            AssociatedTripName = nextTripName,
                            AssociatedTripNumber = nextTripNumber,
                            CoordinateUncertaintyInMeters = s.CoordinateUncertaintyInMeters,
                            Habitat = s.Habitat,
                            Latitude = s.Latitude,
                            Locality = s.Locality,
                            LocationNotes = s.LocationNotes,
                            Longitude = s.Longitude,
                            MinimumElevationInMeters = s.MinimumElevationInMeters,
                            PhotoAsBase64 = s.PhotoAsBase64,
                            SiteName = nextSiteName,
                            SiteNumber = nextSiteNumber
                        });

                        backup.Specimen.Where(x => x.AssociatedSiteNumber == s.SiteNumber).ForEach(async s =>
                        {
                            int nextSpecimenNumber = await specimenService.GetNextCollectionNumber(settingsViewModel.CurrentCollectionCount);
                            string nextSpecimenName = $"{nextSiteNumber}-{nextSpecimenNumber}";

                            await specimenService.CreateAsync(new Specimen()
                            {
                                AdditionalInfo = s.AdditionalInfo,
                                AssociatedSiteName = nextSiteName,
                                AssociatedSiteNumber = nextSiteNumber,
                                Cultivated = s.Cultivated,
                                FieldIdentification = s.FieldIdentification,
                                IndividualCount = s.IndividualCount,
                                LabelString = s.LabelString,
                                LifeStage = s.LifeStage,
                                OccurrenceNotes = s.OccurrenceNotes,
                                PhotoAsBase64 = s.PhotoAsBase64,
                                SpecimenName = nextSpecimenName,
                                SpecimenNumber = nextSpecimenNumber,
                                Substrate = s.Substrate
                            });
                        });
                    });
                });

                backup.Settings.ForEach(async s =>
                {
                    await settingService.CreateAsync(new Setting() 
                    { 
                        LastSaved = s.LastSaved,
                        SettingName = s.SettingName,
                        SettingValue = s.SettingValue
                    });
                });

                backup.ExceptionRecords.ForEach(async er =>
                {
                    await exceptionRecordService.AddAsync(new ExceptionRecord() 
                    { 
                        Created = er.Created,
                        DeviceInfo = er.DeviceInfo, 
                        ExceptionInfo = er.ExceptionInfo
                    });
                });

                result = true;
                streamReader.Dispose();
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