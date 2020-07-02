using collNotes.Services.Data;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collNotes.DeviceServices.Geolocation
{
    public class GeoLocationService : IGeoLocationService
    {
        private const int _TIMEOUT_ = 20;

        public async Task<Location> GetCurrentLocation(IExceptionRecordService exceptionRecordService)
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