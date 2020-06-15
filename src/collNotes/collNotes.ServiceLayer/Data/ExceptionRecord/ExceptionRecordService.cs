using collNotes.Domain.Models;
using collNotes.Ef.Context;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collNotes.ServiceLayer.Data
{
    public class ExceptionRecordService : IExceptionRecordService
    {
        private readonly CollNotesContext context;

        public ExceptionRecordService(CollNotesContext context)
        {
            this.context = context;
        }

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

            await context.ExceptionRecords.AddAsync(exceptionRecord);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ExceptionRecord>> GetAllAsync()
        {
            return await context.ExceptionRecords.ToListAsync();
        }

        public async Task<bool> AddAsync(ExceptionRecord exceptionRecord)
        {
            await context.ExceptionRecords.AddAsync(exceptionRecord);
            return await context.SaveChangesAsync() > 0;
        }
    }
}