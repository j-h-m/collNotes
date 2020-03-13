using System;
using collNotes.Data.Models;
using collNotes.Services;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class NewTripViewModel : BaseViewModel
    {
        public Trip Trip { get; set; }
        public bool IsClone { get; set; }
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
        public TripService tripService;

        /// <summary>
        /// Constructor for a new Trip.
        /// </summary>
        public NewTripViewModel()
        {
            tripService = new TripService(Context);

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
            tripService = new TripService(Context);

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