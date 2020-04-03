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
            
            if (Context.Specimen.Any())
            {
                int lastSpecimenNumber = await Context.Specimen.MaxAsync(s => s.SpecimenNumber);

                specimenCount = lastSpecimenNumber > specimenCount ?
                    lastSpecimenNumber :
                    specimenCount;
            }

            return specimenCount + 1;
        }

        public async Task<bool> UpdateCollectionNumber()
        {
            var collectionCountSetting = await SettingService.GetByNameAsync(CollNotesSettings.CollectionCountKey);
            int specimenCount = (settingsViewModel.CurrentCollectionCount == 0 && Context.Specimen.Any()) ?
                await Context.Specimen.CountAsync() :
                settingsViewModel.CurrentCollectionCount;

            if (collectionCountSetting is Setting)
            {
                collectionCountSetting.SettingValue = specimenCount.ToString();
                return await SettingService.UpdateAsync(collectionCountSetting);
            }
            else
            {
                return await SettingService.CreateAsync(new Setting()
                {
                    SettingName = CollNotesSettings.CollectionCountKey,
                    SettingValue = specimenCount.ToString(),
                    LastSaved = DateTime.Now
                });
            }
        }

        public async Task<Specimen> GetByNameAsync(string name)
        {
            return await Context.Specimen.Where(s => s.SpecimenName == name).FirstOrDefaultAsync();
        }
    }
}