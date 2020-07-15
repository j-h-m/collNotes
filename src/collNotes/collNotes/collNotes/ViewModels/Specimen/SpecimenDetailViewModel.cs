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

        private readonly SettingsViewModel settingsViewModel = 
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public SpecimenDetailViewModel(Specimen specimen = null)
        {
            Title = specimen?.SpecimenName;
            Specimen = specimen;
        }
    }
}