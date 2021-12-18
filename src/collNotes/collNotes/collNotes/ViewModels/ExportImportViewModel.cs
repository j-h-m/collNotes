using System.IO;
using System.Threading.Tasks;
using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.DeviceServices.Connectivity;
using collNotes.DeviceServices.Email;
using collNotes.DeviceServices.Permissions;
using collNotes.DeviceServices;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using collNotes.Services;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using collNotes.Domain.Models;
using static collNotes.DeviceServices.Permissions.PermissionsService;
using System.Collections;
using System.Collections.Generic;
using System;

namespace collNotes.ViewModels
{
    public class ExportImportViewModel : BaseViewModel
    {
        private readonly SettingsViewModel settingsViewModel =
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
        private readonly ICollectionService collectionService =
            DependencyService.Get<ICollectionService>(DependencyFetchTarget.NewInstance);
        private readonly IBackupService backupService =
            DependencyService.Get<IBackupService>(DependencyFetchTarget.NewInstance);
        private readonly IEmailService emailService =
            DependencyService.Get<IEmailService>(DependencyFetchTarget.NewInstance);
        private readonly IShareFileService shareFileService =
            DependencyService.Get<IShareFileService>(DependencyFetchTarget.NewInstance);
        private readonly IPermissionsService permissionsService =
            DependencyService.Get<IPermissionsService>(DependencyFetchTarget.NewInstance);

        private readonly string exportMethod;

        private bool _IsConnectionAvailable;
        public bool IsConnectionAvailable
        {
            get { return _IsConnectionAvailable; }
            set
            {
                _IsConnectionAvailable = value;
                OnPropertyChanged(nameof(IsConnectionAvailable));
            }
        }
        private bool _ShowConnectionErrorMsg;
        public bool ShowConnectionErrorMsg
        {
            get { return _ShowConnectionErrorMsg; }
            set
            {
                _ShowConnectionErrorMsg = value;
                OnPropertyChanged(nameof(ShowConnectionErrorMsg));
            }
        }

        private string _SelectedDirection = "Export";
        public string SelectedDirection
        {
            get { return _SelectedDirection; }
            set
            {
                _SelectedDirection = value;
                OnPropertyChanged(nameof(SelectedDirection));
            }
        }

        private string _SelectedType = "Trip";
        public string SelectedType
        {
            get { return _SelectedType; }
            set
            {
                _SelectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        public IEnumerable<string> Directions
        {
            get { return new List<string>() { "Export", "Import" }; }
        }

        public IEnumerable<string> Types
        {
            get { return new List<string>() { "Backup", "Trip" }; }
        }

        public ExportImportViewModel()
        {
            exportMethod = settingsViewModel.SelectedExportMethod;
        }

        public async Task<bool> ImportTrip(Stream stream)
        {
            return await this.collectionService.ImportCollectionData(stream);
        }

        public async Task<bool> ExportTrip(List<Trip> trips, string exportPath)
        {
            bool result = false;

            if (await CheckOrRequestStoragePermission())
            {
                if (await this.collectionService.ExportCollectionData(trips, exportPath))
                {
                    await ShareOrEmail(exportPath, $"Export.csv");
                    result = true;
                }

            }

            return result;
        }

        public async Task<bool> ImportBackup(Stream stream)
        {
            return await this.backupService.ImportBackup(stream);
        }

        public async Task<bool> ExportBackup(string exportPath)
        {
            if (await CheckOrRequestStoragePermission())
            {
                if (await this.backupService.ExportBackup(exportPath))
                {
                    await ShareOrEmail(exportPath, "Backup.xml");
                }

                return true;
            }
            else { return false; }
        }

        private async Task ShareOrEmail(string exportPath, string title)
        {
            // email doesn't work for some devices
            if (exportMethod.Equals("Email"))
            {
                var result = await this.emailService.SendEmail($"collNotes Export - {title}",
                    "Edit body and add recipients", null, exportPath);
                if (result == EmailService.Result.NotSupported) // email is not supported
                {
                    // try share
                    await this.shareFileService.ShareFile(exportPath, $"collNotes Export - {title}");
                }
            }
            else if (exportMethod.Equals("Multi-Option Share"))
            {
                await this.shareFileService.ShareFile(exportPath, $"collNotes Export - {title}");
            }
            else // default
            {
                await this.shareFileService.ShareFile(exportPath, $"collNotes Export - {title}");
            }
        }

        /// <summary>
        /// if storage permission is granted returns true.
        /// if not granted, returns result of permission request.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckOrRequestStoragePermission()
        {
            bool result;

            result = await this.permissionsService.RequestPermission(PermissionName.Storage);

            return result;
        }
    }
}