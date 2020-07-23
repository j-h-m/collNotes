using collNotes.Domain.Models;
using collNotes.Ef.Context;
using collNotes.Settings;
using collNotes.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace collNotes.Services.Data.RecordData
{
    public class SpecimenService : IServiceBase<Specimen>
    {
        private CollNotesContext Context =
            DependencyService.Get<CollNotesContext>(DependencyFetchTarget.GlobalInstance);

        private readonly SettingsViewModel settingsViewModel =
            DependencyService.Get<SettingsViewModel>(DependencyFetchTarget.GlobalInstance);
        
        public SpecimenService() { }

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

        public bool DeleteAll()
        {
            if (Context.Specimen.Any())
            {
                Context.Specimen.ForEach<Specimen>(async specimen =>
                {
                    await DeleteAsync(specimen);
                });
                return true;
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

        public async Task<int> GetNextCollectionNumber(int currentCollectionCount)
        {
            if (Context.Specimen.Any())
            {
                int maxSpecimenNumber = await Context.Specimen.MaxAsync(s => s.SpecimenNumber);

                return (currentCollectionCount > maxSpecimenNumber) ?
                    currentCollectionCount : maxSpecimenNumber + 1;
            }
            else
            {
                return settingsViewModel.CurrentCollectionCount > 0 ?
                   currentCollectionCount : 0;
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