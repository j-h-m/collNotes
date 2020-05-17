using collNotes.Data.Models;
using collNotes.Factories;
using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Settings;
using collNotes.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace collNotes.ViewModels
{
    public class NewSpecimenViewModel : BaseViewModel
    {
        public Specimen Specimen { get; set; }
        public IEnumerable<Site> AssociableSites { get; set; }
        public string AssociatedSiteName { get; set; }
        public IEnumerable<string> LifeStages { get => CollNotesSettings.LifeStages; }
        public string SelectedLifeStage { get; set; }
        public bool IsClone { get; set; }

        private readonly SiteService siteService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;

        public readonly SpecimenService specimenService;
        public readonly IExceptionRecordService exceptionRecordService;
        public readonly ICameraService cameraService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;

        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = (text, values) =>
            values.Where(x =>
                x.StartsWith(text, StringComparison.CurrentCulture)).
            OrderBy(x => x).
            ToList();

        public NewSpecimenViewModel()
        {
            siteService = new SiteService(Context);
            specimenService = new SpecimenService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            cameraService = new CameraService();
            settingService = new SettingService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            int nextSpecimenNumber = specimenService.GetNextCollectionNumber().Result;
            AssociableSites = siteService.GetAllAsync().Result;

            Specimen = new Specimen()
            {
                SpecimenNumber = nextSpecimenNumber,
                SpecimenName = $"#-{nextSpecimenNumber}"
            };

            Title = Specimen.SpecimenName;
            IsClone = false;
        }

        public NewSpecimenViewModel(Specimen specimenToClone)
        {
            siteService = new SiteService(Context);
            specimenService = new SpecimenService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            cameraService = new CameraService();
            settingService = new SettingService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            int nextSpecimenNumber = specimenService.GetNextCollectionNumber().Result;
            AssociableSites = siteService.GetAllAsync().Result;

            Specimen = new Specimen()
            {
                AdditionalInfo = specimenToClone.AdditionalInfo,
                AssociatedSiteName = specimenToClone.AssociatedSiteName,
                AssociatedSiteNumber = specimenToClone.AssociatedSiteNumber,
                Cultivated = specimenToClone.Cultivated,
                FieldIdentification = specimenToClone.FieldIdentification,
                IndividualCount = specimenToClone.IndividualCount,
                LifeStage = specimenToClone.LifeStage,
                OccurrenceNotes = specimenToClone.OccurrenceNotes,
                SpecimenNumber = nextSpecimenNumber,
                SpecimenName = $"{specimenToClone.AssociatedSiteNumber}-{nextSpecimenNumber}",
                Substrate = specimenToClone.Substrate
            };

            Title = Specimen.SpecimenName;
            IsClone = true;
        }
    }
}