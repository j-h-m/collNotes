using collNotes.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Xamarin.Essentials;

namespace collNotes.Data.Context
{
    public class CollNotesContext : DbContext
    {
        private string SqliteFilePath { get; set; }

        public CollNotesContext()
        {
            this.SqliteFilePath = GetFilePathForDevice();
            this.Database.EnsureCreated();
        }

        public CollNotesContext(DbContextOptions<CollNotesContext> options) : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Specimen> Specimen { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ExceptionRecord> ExceptionRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Filename={SqliteFilePath}");
        }

        /// <summary>
        /// Get the file path for device type.
        /// </summary>
        /// <returns>string file path</returns>
        private string GetFilePathForDevice()
        {
            string fileName = "collnotes.db3";
            string folderDeviceSpecific = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                folderDeviceSpecific = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                folderDeviceSpecific = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }

            return Path.Combine(folderDeviceSpecific, fileName);
        }
    }
}