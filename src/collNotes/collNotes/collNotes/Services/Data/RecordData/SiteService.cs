using collNotes.Domain.Models;
using collNotes.Ef.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace collNotes.Services.Data.RecordData
{
    public class SiteService : IServiceBase<Site>
    {
        private CollNotesContext Context =
            DependencyService.Get<CollNotesContext>(DependencyFetchTarget.GlobalInstance);

        public SiteService() { }

        public async Task<bool> CreateAsync(Site site)
        {
            await Context.Sites.AddAsync(site);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateMultipleAsync(IEnumerable<Site> sites)
        {
            await Context.Sites.AddRangeAsync(sites);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<List<Specimen>> GetChildrenAsync(Site site)
        {
            return await Context.Specimen.Where(s => s.AssociatedSiteName.Equals(site.SiteName)).ToListAsync();
        }

        public async Task<bool> DeleteAsync(Site site)
        {
            if (site is Site)
            {
                var assocSpecimen = Context.Specimen.Where(s => s.AssociatedSiteName.Equals(site.SiteName));

                Context.Specimen.RemoveRange(assocSpecimen);
                Context.Sites.Remove(site);

                return await Context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public bool DeleteAll()
        {
            if (Context.Sites.Any())
            {
                Context.Sites.ForEach<Site>(async site =>
                {
                    await DeleteAsync(site);
                });
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Site>> GetAllAsync()
        {
            return await Context.Sites.ToListAsync();
        }

        public async Task<Site> GetAsync(int id)
        {
            return await Context.Sites.Where(s =>
                s.SiteID == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Site site)
        {
            Context.Sites.Update(site);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetNextCollectionNumber(int currentCollectionCount = 0)
        {
            int siteCount = await Context.Sites.CountAsync();
            if (Context.Sites.Any())
            {
                int lastSiteNumber = await Context.Sites.MaxAsync(s => s.SiteNumber);
                if (siteCount < lastSiteNumber)
                    return lastSiteNumber + 1;
            }
            return siteCount + 1;
        }

        public async Task<Site> GetByNameAsync(string name)
        {
            return await Context.Sites.Where(s =>
                s.SiteName == name).FirstOrDefaultAsync();
        }
    }
}