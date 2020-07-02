using System.Threading.Tasks;

namespace collNotes.DeviceServices
{
    public interface IShareFileService
    {
        Task ShareFile(string filePath, string title);
    }
}
