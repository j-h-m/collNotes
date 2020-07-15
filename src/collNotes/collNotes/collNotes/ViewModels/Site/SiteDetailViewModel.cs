using collNotes.ColorThemes.ConfigFactory;
using collNotes.DeviceServices.AppTheme;
using collNotes.Domain.Models;
using collNotes.Services;
using collNotes.Services.Data;
using collNotes.Services.Data.RecordData;
using System.Collections.Generic;
using Xamarin.Forms;

namespace collNotes.ViewModels
{
    public class SiteDetailViewModel : BaseViewModel
    {
        public Site Site { get; set; }
        public IEnumerable<Trip> AssociableTrips { get; set; }
        public string AssociatedTripName { get; set; }

        private readonly TripService tripService =
            DependencyService.Get<TripService>(DependencyFetchTarget.NewInstance);

        public SiteDetailViewModel(Site site)
        {
            Site = site;
            Title = Site?.SiteName;
            AssociatedTripName = Site?.AssociatedTripName;

            AssociableTrips = tripService.GetAllAsync().Result;
        }
    }
}