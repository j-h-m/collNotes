using collNotes.Data.Models;
using collNotes.Services;

namespace collNotes.ViewModels
{
    public class SpecimenDetailViewModel : BaseViewModel
    {
        public Specimen Specimen { get; set; }
        public SpecimenService SpecimenService { get; set; }

        public SpecimenDetailViewModel(Specimen specimen = null)
        {
            SpecimenService = new SpecimenService(Context);

            Title = specimen?.SpecimenName;
            Specimen = specimen;
        }
    }
}