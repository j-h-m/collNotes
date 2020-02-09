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
        public bool IsCultivated { get; set; }
        public bool IsClone { get; set; }

        private SiteService SiteService { get; set; }
        public SpecimenService SpecimenService { get; set; }
        public ExceptionRecordService ExceptionRecordService { get; set; }
        public CameraService CameraService { get; set; }

        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = (text, values) =>
            values.Where(x =>
                x.StartsWith(text, StringComparison.CurrentCulture)).
            OrderBy(x => x).
            ToList();

        public NewSpecimenViewModel()
        {
            SiteService = new SiteService(Context);
            SpecimenService = new SpecimenService(Context);
            ExceptionRecordService = new ExceptionRecordService(Context);
            CameraService = new CameraService();

            int nextSpecimenNumber = SpecimenService.GetNextCollectionNumber().Result;
            AssociableSites = SiteService.GetAllAsync().Result;

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
            SiteService = new SiteService(Context);
            SpecimenService = new SpecimenService(Context);

            int nextSpecimenNumber = SpecimenService.GetNextCollectionNumber().Result;
            AssociableSites = SiteService.GetAllAsync().Result;

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