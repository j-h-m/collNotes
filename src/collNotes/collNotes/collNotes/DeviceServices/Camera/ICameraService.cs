using collNotes.Services.Data;
using System.Threading.Tasks;

namespace collNotes.DeviceServices.Camera
{
    public interface ICameraService
    {
        Task<string> TakePicture(IExceptionRecordService exceptionRecordService, string photoName);
    }
}