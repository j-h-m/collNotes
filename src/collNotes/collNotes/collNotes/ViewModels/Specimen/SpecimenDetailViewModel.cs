using collNotes.Data.Models;
using collNotes.Factories;
using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Settings;

namespace collNotes.ViewModels
{
    public class SpecimenDetailViewModel : BaseViewModel
    {
        public Specimen Specimen { get; set; }

        private readonly IExceptionRecordService exceptionRecordService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;
        
        public readonly SiteService siteService;
        public readonly SpecimenService specimenService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;

        public SpecimenDetailViewModel(Specimen specimen = null)
        {
            specimenService = new SpecimenService(Context);
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