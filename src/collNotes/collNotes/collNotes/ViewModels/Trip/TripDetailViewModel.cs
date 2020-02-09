using collNotes.Data.Models;
using collNotes.Services;
using System;

namespace collNotes.ViewModels
{
    public class TripDetailViewModel : BaseViewModel
    {
        public Trip Trip { get; set; }
        public TripService TripService { get; set; }

        public TripDetailViewModel(Trip trip)
        {
            if (trip is Trip)
            {
                TripService = new TripService(Context);

                Trip = trip;
                Title = Trip?.TripName;
            }
            else
            {
                throw new ArgumentNullException(nameof(trip));
            }
        }
    }
}