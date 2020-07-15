using System;
using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        public Trip Trip { get; set; }
        public bool IsClone { get; set; }
        
        private readonly SettingsViewModel settingsViewModel = 
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
        private readonly TripService tripService =
            DependencyService.Get<TripService>(DependencyFetchTarget.NewInstance);

        /// <summary>
        /// Constructor for a new Trip.
        /// </summary>
        public NewTripViewModel()
        {
            int nextTripNumber = tripService.GetNextCollectionNumber().Result;

            Trip = new Trip()
            {
                TripName = $"Trip-{nextTripNumber}",
                PrimaryCollector = settingsViewModel.CurrentCollectorName,
                CollectionDate = DateTime.Now,
                TripNumber = nextTripNumber
            };

            Title = Trip.TripName;
            IsClone = false;
        }

        /// <summary>
        /// Constructor for a Trip clone.
        /// </summary>
        /// <param name="tripToClone">The trip to clone.</param>
        public NewTripViewModel(Trip tripToClone)
        {
            int nextTripNumber = tripService.GetNextCollectionNumber().Result;

            Trip = new Trip()
            {
                TripName = $"Trip-{nextTripNumber}",
                PrimaryCollector = tripToClone.PrimaryCollector,
                AdditionalCollectors = tripToClone.AdditionalCollectors,
                CollectionDate = tripToClone.CollectionDate,
                TripNumber = nextTripNumber
            };

            Title = Trip.TripName;
            IsClone = true;
        }
    }
}