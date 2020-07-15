using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
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

        private readonly TripService tripService =
            DependencyService.Get<TripService>(DependencyFetchTarget.NewInstance);
        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public TripsViewModel()
        {
            Title = "Trips";
            Trips = new ObservableCollection<Trip>();

            LoadTripsCommand = new Command(async () => await ExecuteLoadTripsCommand());

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteTrips", (sender) =>
            {
                tripService.DeleteAll();
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