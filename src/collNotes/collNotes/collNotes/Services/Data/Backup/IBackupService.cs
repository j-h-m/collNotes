using System.IO;
using System.Threading.Tasks;

namespace collNotes.Services.Data
{
    public interface IBackupService
    {
        Task<bool> ExportBackup(string exportFilePath);
        Task<bool> ImportBackup(Stream stream);
    }
}