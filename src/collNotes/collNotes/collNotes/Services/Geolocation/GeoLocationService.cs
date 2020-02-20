using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collNotes.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private const int _TIMEOUT_ = 20;

        public async Task<Location> GetCurrentLocation(ExceptionRecordService exceptionRecordService)
        {
            Location currentLocation = null;

            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(_TIMEOUT_)));

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