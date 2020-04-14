using collNotes.Data.Models;
using collNotes.Factories;
using collNotes.Services;
using collNotes.Services.AppTheme;
using collNotes.Services.Settings;
using collNotes.Views;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace collNotes.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        public ObservableCollection<Trip> Trips { get; set; }
        public Command LoadTripsCommand { get; set; }

        private readonly IExceptionRecordService exceptionRecordService;
        private readonly IAppThemeService appThemeService;
        private readonly ISettingService settingService;

        public readonly TripService tripService;
        public readonly XfMaterialColorConfigFactory xfMaterialColorConfigFactory;

        public TripsViewModel()
        {
            tripService = new TripService(Context);
            settingService = new SettingService(Context);
            exceptionRecordService = new ExceptionRecordService(Context);
            appThemeService = new AppThemeService(settingService, exceptionRecordService);
            xfMaterialColorConfigFactory = new XfMaterialColorConfigFactory(appThemeService);

            Title = "Trips";
            Trips = new ObservableCollection<Trip>();

            LoadTripsCommand = new Command(async () => await ExecuteLoadTripsCommand());

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteTrips", async (sender) =>
            {
                await tripService.DeleteAllAsync();
            });
        }

        private async Task ExecuteLoadTripsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Trips.Clear();
                var trips = await tripService.GetAllAsync();
                trips = trips.OrderBy(trip => trip.TripNumber);

                trips.ForEach(trip => Trips.Add(trip));
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