using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace collNotes.ViewModels
{
    public class SpecimenViewModel : BaseViewModel
    {
        public ObservableCollection<Specimen> SpecimenCollection { get; set; }
        public Command LoadSpecimenCommand { get; set; }
        public SiteService siteService;
        public SpecimenService specimenService;
        private IExceptionRecordService exceptionRecordService;

        public SpecimenViewModel()
        {
            specimenService = new SpecimenService(Context);
            siteService = new SiteService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);

            Title = "Specimen";
            SpecimenCollection = new ObservableCollection<Specimen>();

            LoadSpecimenCommand = new Command(async () => await ExecuteLoadSpecimenCommand());
        }

        private async Task ExecuteLoadSpecimenCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                SpecimenCollection.Clear();
                var specimenCollection = await specimenService.GetAllAsync();
                specimenCollection = specimenCollection.OrderBy(specimen => specimen.AssociatedSiteNumber);

                specimenCollection.ForEach(specimen =>
                {
                    specimen.LabelString = string.IsNullOrEmpty(specimen.FieldIdentification) ?
                        specimen.SpecimenName : specimen.FieldIdentification;
                    SpecimenCollection.Add(specimen);
                });
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}