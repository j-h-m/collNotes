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
        
        public readonly SiteService siteService;
        public readonly SpecimenService specimenService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;

        private readonly IExceptionRecordService exceptionRecordService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public SpecimenViewModel()
        {
            specimenService = new SpecimenService(Context, settingsViewModel);
            siteService = new SiteService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            settingService = new SettingService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

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