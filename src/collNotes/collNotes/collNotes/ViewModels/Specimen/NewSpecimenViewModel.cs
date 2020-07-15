using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.DeviceServices.Camera;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using collNotes.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        private readonly SettingsViewModel settingsViewModel = 
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
        private readonly SiteService siteService =
            DependencyService.Get<SiteService>(DependencyFetchTarget.NewInstance);
        public readonly SpecimenService specimenService =
            DependencyService.Get<SpecimenService>(DependencyFetchTarget.NewInstance);

        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = 
            (text, values) => values.Where(x => x.StartsWith(text, StringComparison.CurrentCulture))
            .OrderBy(x => x)
            .ToList();

        public NewSpecimenViewModel()
        {
            int nextSpecimenNumber = specimenService.GetNextCollectionNumber(settingsViewModel.CurrentCollectionCount).Result;
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
            int nextSpecimenNumber = specimenService.GetNextCollectionNumber(settingsViewModel.CurrentCollectionCount).Result;
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