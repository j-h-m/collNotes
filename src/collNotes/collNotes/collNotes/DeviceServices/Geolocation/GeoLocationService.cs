using collNotes.Services.Data;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace collNotes.DeviceServices.Geolocation
{
    public class GeoLocationService : IGeoLocationService
    {
        private const int _TIMEOUT_ = 20;

        private readonly IExceptionRecordService exceptionRecordService =
            DependencyService.Get<IExceptionRecordService>(DependencyFetchTarget.NewInstance);

        public async Task<Location> GetCurrentLocation()
        {
            Location currentLocation = null;

            try
            {
                var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(
                    new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(_TIMEOUT_)));

                if (location != null)
                    currentLocation = location;
            }
            catch (Exception ex)
            {
                await exceptionRecordService.CreateExceptionRecord(ex);
            }

            return currentLocation;
        }
    }
}