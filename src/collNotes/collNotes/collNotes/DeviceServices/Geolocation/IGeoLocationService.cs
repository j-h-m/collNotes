using collNotes.Services.Data;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collNotes.DeviceServices.Geolocation
{
    public interface IGeoLocationService
    {
        Task<Location> GetCurrentLocation(IExceptionRecordService exceptionRecordService);
    }
}