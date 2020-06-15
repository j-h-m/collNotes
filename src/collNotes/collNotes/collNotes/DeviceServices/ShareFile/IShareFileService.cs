using System.Threading.Tasks;

namespace collNotes.DeviceServices.ShareFile
{
    public interface IShareFileService
    {
        Task ShareFile(string filePath, string title);
    }
}
