using collNotes.Data.Context;
using collNotes.Data.Models;
using collNotes.Services.Settings;
using collNotes.Settings;
using collNotes.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace collNotes.Services
{
    public class SpecimenService : IServiceBase<Specimen>
    {
        private CollNotesContext Context { get; set; }
        private SettingService SettingService { get; set; }
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
        public SpecimenService(CollNotesContext collNotesContext)
        {
            Context = collNotesContext;
            SettingService = new SettingService(collNotesContext);
        }

        public async Task<bool> CreateAsync(Specimen specimen)
        {
            await Context.Specimen.AddAsync(specimen);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateMultipleAsync(IEnumerable<Specimen> specimen)
        {
            await Context.Specimen.AddRangeAsync(specimen);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Specimen specimen)
        {
            if (specimen is Specimen)
            {
                Context.Specimen.Remove(specimen);
                return await Context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<Specimen>> GetAllAsync(bool forceRefresh = false)
        {
            return await Context.Specimen.ToListAsync();
        }

        public async Task<Specimen> GetAsync(int id)
        {
            return await Context.Specimen.Where(s => s.SpecimenID == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Specimen specimen)
        {
            Context.Specimen.Update(specimen);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetNextCollectionNumber()
        {
            int specimenCount = settingsViewModel.CurrentCollectionCount == 0 ?
                await Context.Specimen.CountAsync() :
                settingsViewModel.CurrentCollectionCount;

            var collectionCountSetting = await SettingService.GetByNameAsync(CollNotesSettings.CollectionCountKey);
            if (collectionCountSetting is Setting)
            {
                collectionCountSetting.SettingValue = (specimenCount + 1).ToString();
                await SettingService.UpdateAsync(collectionCountSetting);
            }
            else
            {
                await SettingService.CreateAsync(new Setting()
                {
                    SettingName = CollNotesSettings.CollectionCountKey,
                    SettingValue = (specimenCount + 1).ToString(),
                    LastSaved = DateTime.Now
                });
            }

            return settingsViewModel.CurrentCollectionCount = specimenCount + 1;
        }

        public async Task<Specimen> GetByNameAsync(string name)
        {
            return await Context.Specimen.Where(s => s.SpecimenName == name).FirstOrDefaultAsync();
        }
    }
}