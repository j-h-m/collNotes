using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class SpecimenDetailViewModel : BaseViewModel
    {
        public Specimen Specimen { get; set; }

        private readonly IExceptionRecordService exceptionRecordService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public readonly SiteService siteService;
        public readonly SpecimenService specimenService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;

        public SpecimenDetailViewModel(Specimen specimen = null)
        {
            specimenService = new SpecimenService(Context, settingsViewModel);
            siteService = new SiteService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            settingService = new SettingService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            Title = specimen?.SpecimenName;
            Specimen = specimen;
        }
    }
}