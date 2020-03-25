using System.IO;
using System.Threading.Tasks;
using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Services.Connectivity;
using collNotes.Services.Data;
using collNotes.Services.Permissions;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace collNotes.ViewModels
{
    public class ExportImportViewModel : BaseViewModel
    {
        private readonly IBackupService backupService;
        private readonly ICollectionService collectionService;
        private readonly IEmailService emailService;
        private readonly IShareFileService shareFileService;
        private readonly IPermissionsService permissionsService;
        private readonly string exportMethod;
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public readonly TripService tripService;
        public readonly IExceptionRecordService exceptionRecordService;
        public readonly IConnectivityService connectivityService;
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
            backupService = new BackupService(Context);
            collectionService = new CollectionService(Context);
            emailService = new EmailService(Context);
            tripService = new TripService(Context);
            shareFileService = new ShareFileService();
            permissionsService = new PermissionsService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            connectivityService = new ConnectivityService();

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
                bool result = await this.collectionService.ExportCollectionData(trip, exportPath);
                if (result)
                    await ShareOrEmail(exportPath, trip.TripName);

                return result;
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
                bool result = await this.backupService.ExportBackup(exportPath);
                if (result)
                    await ShareOrEmail(exportPath, "Backup");

                return result;
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

        private async Task<bool> CheckOrRequestStoragePermission()
        {
            bool result;
            if (!(result = await this.permissionsService.CheckStoragePermission()))
            {
                if (!(result = await this.permissionsService.RequestStoragePermission()))
                {
                    await MaterialDialog.Instance.AlertAsync("Storage permission is required for data export");
                }
            }
            return result;
        }
    }
}