using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices;
using collNotes.DeviceServices.AppTheme;
using collNotes.DeviceServices.Camera;
using collNotes.DeviceServices.Connectivity;
using collNotes.DeviceServices.Email;
using collNotes.DeviceServices.Geolocation;
using collNotes.DeviceServices.Permissions;
using collNotes.Ef.Context;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using collNotes.Services.MenuPage;
using collNotes.Settings;
using collNotes.Settings.AutoComplete;
using collNotes.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes
{
    public static class Startup
    {
        public static void Init()
        {
            if (Device.RuntimePlatform.Equals(Device.iOS))
            {
                SQLitePCL.Batteries.Init();
            }
            // Ef Context
            DependencyService.Register<CollNotesContext>();

            // View Models
            DependencyService.Register<SettingsViewModel>();

            // Device Services
            DependencyService.Register<IAppThemeService, AppThemeService>();
            DependencyService.Register<ICameraService, CameraService>();
            DependencyService.Register<IConnectivityService, ConnectivityService>();
            DependencyService.Register<IEmailService, EmailService>();
            DependencyService.Register<IGeoLocationService, GeoLocationService>();
            DependencyService.Register<IPermissionsService, PermissionsService>();
            DependencyService.Register<IShareFileService, ShareFileService>();

            // Domain Services
            DependencyService.Register<SiteService>();
            DependencyService.Register<SpecimenService>();
            DependencyService.Register<TripService>();
            DependencyService.Register<ISettingService, SettingService>();
            DependencyService.Register<IExceptionRecordService, ExceptionRecordService>();
            DependencyService.Register<I_ImportRecordService, ImportRecordService>();

            // Data Operation Services
            DependencyService.Register<IBackupService, BackupService>();
            DependencyService.Register<ICollectionService, CollectionService>();

            // Other
            DependencyService.Register<XfMaterialColorConfigFactory>();
            DependencyService.Register<MenuPageService>();
        }
    }
}