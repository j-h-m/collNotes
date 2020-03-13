using System.Threading.Tasks;

namespace collNotes.Services
{
    public interface ICameraService
    {
        Task<string> TakePicture(IExceptionRecordService exceptionRecordService, string photoName);
    }
}