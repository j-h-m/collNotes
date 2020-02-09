using System.Threading.Tasks;

namespace collNotes.Services
{
    public interface ICameraService
    {
        Task<string> TakePicture(ExceptionRecordService exceptionRecordService, string photoName);
    }
}