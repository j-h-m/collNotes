using collNotes.Data.Context;
using collNotes.Data.Models;
using collNotes.Services.Settings;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace collNotes.Services.Data
{
    public class BackupService : IBackupService
    {
        private readonly TripService tripService;
        private readonly SiteService siteService;
        private readonly SpecimenService specimenService;
        private readonly SettingService settingService;
        private readonly ExceptionRecordService exceptionRecordService;

        public BackupService(CollNotesContext collNotesContext)
        {
            tripService = new TripService(collNotesContext);
            siteService = new SiteService(collNotesContext);
            specimenService = new SpecimenService(collNotesContext);
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
                    await tripService.CreateAsync(t);
                });

                backup.Sites.ForEach(async s =>
                {
                    await siteService.CreateAsync(s);
                });

                backup.Specimen.ForEach(async s =>
                {
                    await specimenService.CreateAsync(s);
                });

                backup.Settings.ForEach(async s =>
                {
                    await settingService.CreateAsync(s);
                });

                backup.ExceptionRecords.ForEach(async er =>
                {
                    await exceptionRecordService.AddAsync(er);
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