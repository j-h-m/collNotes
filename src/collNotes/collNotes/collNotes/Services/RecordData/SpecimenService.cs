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
        private readonly SettingsViewModel settingsViewModel = DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);

        public SpecimenService(CollNotesContext collNotesContext)
        {
            Context = collNotesContext;
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

        public async Task<IEnumerable<Specimen>> GetAllAsync()
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
            if (Context.Specimen.Any())
            {
                int maxSpecimenNumber = await Context.Specimen.MaxAsync(s => s.SpecimenNumber);
                if (settingsViewModel.CurrentCollectionCount > maxSpecimenNumber)
                {
                    return settingsViewModel.CurrentCollectionCount;
                }
                else
                {
                    return maxSpecimenNumber + 1;
                }
            }
            else
            {
                return 1;
            }
        }

        public async Task<bool> UpdateCollectionNumber()
        {
            if (Context.Specimen.Any())
            {
                int maxSpecimenNumber = await Context.Specimen.MaxAsync(s => s.SpecimenNumber);
                int currentCollectionCount = settingsViewModel.CurrentCollectionCount;

                int specimenCount = maxSpecimenNumber > currentCollectionCount ?
                    maxSpecimenNumber : currentCollectionCount;
                // increment
                specimenCount += 1;

                settingsViewModel.CurrentCollectionCountString = specimenCount.ToString();

                return await settingsViewModel.CreateOrUpdateSetting(CollNotesSettings.CollectionCountKey,
                                                        specimenCount.ToString());
            }
            else
            {
                return false;
            }
        }

        public async Task<Specimen> GetByNameAsync(string name)
        {
            return await Context.Specimen.Where(s => s.SpecimenName == name).FirstOrDefaultAsync();
        }
    }
}