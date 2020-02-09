using System.Threading.Tasks;

namespace collNotes.Services
{
    public interface IShareFileService
    {
        Task ShareFile(string filePath, string title);
    }
}
