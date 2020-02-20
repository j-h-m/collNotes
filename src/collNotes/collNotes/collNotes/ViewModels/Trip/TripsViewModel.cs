using collNotes.Data.Models;
using collNotes.Services;
using collNotes.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class TripsViewModel : BaseViewModel
    {
        public ObservableCollection<Trip> Trips { get; set; }
        public Command LoadTripsCommand { get; set; }
        public TripService TripService { get; set; }
        private ExceptionRecordService ExceptionRecordService { get; set; }

        public TripsViewModel()
        {
            TripService = new TripService(Context);
            ExceptionRecordService = new ExceptionRecordService(Context);

            Title = "Trips";
            Trips = new ObservableCollection<Trip>();

            LoadTripsCommand = new Command(async () => await ExecuteLoadTripsCommand());

            MessagingCenter.Subscribe<SettingsPage>(this, "DeleteTrips", async (sender) =>
            {
                await TripService.DeleteAllAsync();
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
                var trips = await TripService.GetAllAsync(true);
                foreach (var trip in trips)
                {
                    Trips.Add(trip);
                }
            }
            catch (Exception ex)
            {
                await ExceptionRecordService.CreateExceptionRecord(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}