using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private SiteService siteService;
        public SpecimenService specimenService;
        public IExceptionRecordService exceptionRecordService;
        public ICameraService cameraService;

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