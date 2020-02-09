using collNotes.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace collNotes.Services.Settings
{
    public interface ISettingService
    {
        Task<bool> CreateAsync(Setting setting);

        Task<bool> UpdateAsync(Setting setting);

        Task<bool> DeleteAsync(Setting setting);

        Task<bool> DeleteAllAsync();

        Task<Setting> GetByNameAsync(string name);

        Task<IEnumerable<Setting>> GetAllAsync();
    }
}