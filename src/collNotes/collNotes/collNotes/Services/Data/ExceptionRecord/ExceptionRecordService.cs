using collNotes.Domain.Models;
using collNotes.Ef.Context;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.Services.Data
{
    public class ExceptionRecordService : IExceptionRecordService
    {
        private readonly CollNotesContext Context =
            DependencyService.Get<CollNotesContext>(DependencyFetchTarget.GlobalInstance);

        public ExceptionRecordService() { }

        public async Task<bool> CreateExceptionRecord(Exception exception)
        {
            ExceptionRecord exceptionRecord = new ExceptionRecord()
            {
                Created = DateTime.Now,
                DeviceInfo = $"Manufacturer: {DeviceInfo.Manufacturer}; " +
                    $"Model: {DeviceInfo.Model}; " +
                    $"Name: {DeviceInfo.Name}; " +
                    $"Platform: {DeviceInfo.Platform}; " +
                    $"Version: {DeviceInfo.VersionString}",
                ExceptionInfo = $"Message: {exception.Message}; " +
                    $"Source: {exception.Source}; " +
                    $"StackTrace: {exception.StackTrace}; " +
                    $"HelpLink: {exception.HelpLink}"
            };

            Crashes.TrackError(exception); // track error in app center

            await Context.ExceptionRecords.AddAsync(exceptionRecord);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ExceptionRecord>> GetAllAsync()
        {
            return await Context.ExceptionRecords.ToListAsync();
        }

        public async Task<bool> AddAsync(ExceptionRecord exceptionRecord)
        {
            await Context.ExceptionRecords.AddAsync(exceptionRecord);
            return await Context.SaveChangesAsync() > 0;
        }
    }
}