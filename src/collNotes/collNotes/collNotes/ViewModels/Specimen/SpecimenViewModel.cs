using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
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
        
        private readonly SettingsViewModel settingsViewModel = 
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
        private readonly SpecimenService specimenService =
            DependencyService.Get<SpecimenService>(DependencyFetchTarget.NewInstance);
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public SpecimenViewModel()
        {
            Title = "Specimen";
            SpecimenCollection = new ObservableCollection<Specimen>();

            LoadSpecimenCommand = new Command(async () => await ExecuteLoadSpecimenCommand());

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteSpecimen", (sender) =>
            {
                specimenService.DeleteAll();
            });
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
                specimenCollection = specimenCollection
                    .OrderBy(specimen => specimen.AssociatedSiteNumber)
                    .ThenBy(specimen => specimen.SpecimenNumber);

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