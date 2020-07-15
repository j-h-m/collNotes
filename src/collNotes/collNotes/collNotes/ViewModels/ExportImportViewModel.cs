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

        public ExportImportViewModel()
        {
            exportMethod = settingsViewModel.SelectedExportMethod;
        }

        public async Task<bool> ImportTrip(Stream stream)
        {
            return await this.collectionService.ImportCollectionData(stream);
        }

        public async Task<bool> ExportTrip(Trip trip, string exportPath)
        {
            if (await CheckOrRequestStoragePermission())
            {
                if (await this.collectionService.ExportCollectionData(trip, exportPath))
                {
                    await ShareOrEmail(exportPath, trip.TripName);
                }

                return true;
            }
            else { return false; }
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
                    await ShareOrEmail(exportPath, "Backup");
                }

                return true;
            }
            else { return false; }
        }

        private async Task ShareOrEmail(string exportPath, string title)
        {
            if (exportMethod.Equals("Email"))
            {
                await this.emailService.SendEmail($"collNotes Export - {title}",
                    "Edit body and add recipients", null, exportPath);
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

            if (!(result = await this.permissionsService.CheckPermission(PermissionName.Storage)))
            {
                result = await this.permissionsService.RequestPermission(PermissionName.Storage);
            }

            return result;
        }
    }
}