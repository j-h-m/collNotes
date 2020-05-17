using System.Collections.Generic;
using System.Threading.Tasks;

namespace collNotes.Services
{
    public interface IServiceBase<T>
    {
        Task<bool> CreateAsync(T thing);

        Task<bool> CreateMultipleAsync(IEnumerable<T> things);

        Task<bool> UpdateAsync(T thing);

        Task<bool> DeleteAsync(T thing);

        Task<T> GetAsync(int id);

        Task<T> GetByNameAsync(string name);

        Task<IEnumerable<T>> GetAllAsync();

        Task<int> GetNextCollectionNumber();
    }
}