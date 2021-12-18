using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using collNotes.Domain.Models;

namespace collNotes.Services.Data
{
    public interface ICollectionService
    {
        Task<bool> ExportCollectionData(List<Trip> trips, string csvPath);
        Task<bool> ImportCollectionData(Stream stream);
    }
}