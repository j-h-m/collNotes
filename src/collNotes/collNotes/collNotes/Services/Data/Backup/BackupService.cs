using collNotes.Domain.Models;
using collNotes.Ef.Context;
using collNotes.Services.Data.RecordData;
using collNotes.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace collNotes.Services.Data
{
    public class BackupService : IBackupService
    {
        private readonly TripService tripService;
        private readonly SiteService siteService;
        private readonly SpecimenService specimenService;
        private readonly ISettingService settingService;
        private readonly IExceptionRecordService exceptionRecordService;
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public BackupService(CollNotesContext collNotesContext)
        {
            tripService = new TripService(collNotesContext);
            siteService = new SiteService(collNotesContext);
            specimenService = new SpecimenService(collNotesContext, settingsViewModel);
            settingService = new SettingService(collNotesContext);
            exceptionRecordService = new ExceptionRecordService(collNotesContext);
        }

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
                    await tripService.CreateAsync(new Trip()
                    {
                        AdditionalCollectors = t.AdditionalCollectors,
                        CollectionDate = t.CollectionDate,
                        PrimaryCollector = t.PrimaryCollector,
                        TripName = t.TripName,
                        TripNumber = t.TripNumber
                    });
                });

                backup.Sites.ForEach(async s =>
                {
                    await siteService.CreateAsync(new Site()
                    {
                        AssociatedTaxa = s.AssociatedTaxa,
                        AssociatedTripName = s.AssociatedTripName,
                        CoordinateUncertaintyInMeters = s.CoordinateUncertaintyInMeters,
                        Habitat = s.Habitat,
                        Latitude = s.Latitude,
                        Locality = s.Locality,
                        LocationNotes = s.LocationNotes,
                        Longitude = s.Longitude,
                        MinimumElevationInMeters = s.MinimumElevationInMeters,
                        PhotoAsBase64 = s.PhotoAsBase64,
                        SiteName = s.SiteName,
                        SiteNumber = s.SiteNumber
                    });
                });

                backup.Specimen.ForEach(async s =>
                {
                    await specimenService.CreateAsync(new Specimen() 
                    { 
                        AdditionalInfo = s.AdditionalInfo,
                        AssociatedSiteName = s.AssociatedSiteName,
                        AssociatedSiteNumber = s.AssociatedSiteNumber,
                        Cultivated = s.Cultivated,
                        FieldIdentification = s.FieldIdentification,
                        IndividualCount = s.IndividualCount,
                        LabelString = s.LabelString,
                        LifeStage = s.LifeStage,
                        OccurrenceNotes = s.OccurrenceNotes,
                        PhotoAsBase64 = s.PhotoAsBase64,
                        SpecimenName = s.SpecimenName,
                        SpecimenNumber = s.SpecimenNumber,
                        Substrate = s.Substrate
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