using System.Threading.Tasks;
using collNotes.Data.Models;

namespace collNotes.Services.Data
{
    public interface ICollectionService
    {
        Task<bool> ExportCollectionData(Trip trip, string csvPath);
    }
}