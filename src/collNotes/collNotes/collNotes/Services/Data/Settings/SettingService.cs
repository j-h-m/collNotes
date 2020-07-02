using collNotes.Domain.Models;
using collNotes.Ef.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace collNotes.Services.Data
{
    public class SettingService : ISettingService
    {
        private CollNotesContext Context { get; set; }

        public SettingService(CollNotesContext collNotesContext)
        {
            Context = collNotesContext;
        }

        public async Task<bool> CreateAsync(Setting setting)
        {
            await Context.Settings.AddAsync(setting);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Setting setting)
        {
            Context.Settings.Update(setting);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Setting setting)
        {
            if (setting is Setting)
            {
                Context.Settings.Remove(setting);
                return await Context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteAllAsync()
        {
            if (Context.Settings.Any())
            {
                Context.Settings.RemoveRange(Context.Settings);
                return await Context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Setting> GetByNameAsync(string name)
        {
            return await Context.Settings.Where(s => s.SettingName == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Setting>> GetAllAsync()
        {
            return await Context.Settings.ToListAsync();
        }
    }
}