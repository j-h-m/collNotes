using collNotes.Data.Models;
using collNotes.Services;

namespace collNotes.ViewModels
{
    public class SpecimenDetailViewModel : BaseViewModel
    {
        public Specimen Specimen { get; set; }
        public SpecimenService specimenService;

        public SpecimenDetailViewModel(Specimen specimen = null)
        {
            specimenService = new SpecimenService(Context);

            Title = specimen?.SpecimenName;
            Specimen = specimen;
        }
    }
}