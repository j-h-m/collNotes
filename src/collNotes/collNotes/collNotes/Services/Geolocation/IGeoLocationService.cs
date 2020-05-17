using System.Threading.Tasks;
using Xamarin.Essentials;

namespace collNotes.Services
{
    public interface IGeoLocationService
    {
        Task<Location> GetCurrentLocation(IExceptionRecordService exceptionRecordService);
    }
}