using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using System;

namespace collNotes.ViewModels
{
    public class TripDetailViewModel : BaseViewModel
    {
        public Trip Trip { get; set; }

        public string originalTripName { get; set; }

        public TripDetailViewModel(Trip trip)
        {
            Trip = trip;
            Title = originalTripName = Trip?.TripName;
        }
    }
}