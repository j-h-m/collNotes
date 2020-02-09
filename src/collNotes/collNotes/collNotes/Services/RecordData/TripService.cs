using collNotes.Data.Context;
using collNotes.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace collNotes.Services
{
    public class TripService : IServiceBase<Trip>
    {
        private CollNotesContext Context { get; set; }

        public TripService(CollNotesContext collNotesContext)
        {
            Context = collNotesContext;
        }

        public async Task<bool> CreateAsync(Trip trip)
        {
            await Context.Trips.AddAsync(trip);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateMultipleAsync(IEnumerable<Trip> trips)
        {
            await Context.Trips.AddRangeAsync(trips);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<Dictionary<Site, List<Specimen>>> GetChildrenAsync(Trip trip)
        {
            var children = new Dictionary<Site, List<Specimen>>();

            if (trip is { })
            {
                var assocSites = Context.Sites.Where(s => s.AssociatedTripName == trip.TripName);
                if (assocSites.Count() > 0)
                {
                    await assocSites.ForEachAsync(async site =>
                    {
                        var associatedSpecimen = Context.Specimen.Where(s => s.AssociatedSiteName == site.SiteName);
                        children.Add(site, await associatedSpecimen.ToListAsync());
                    });
                }
            }

            return children;
        }

        public async Task<bool> DeleteAsync(Trip trip)
        {
            if (trip is Trip)
            {
                var assocSites = Context.Sites.Where(s => s.AssociatedTripName == trip.TripName);
                var assocSiteNames = assocSites.Select(s => s.SiteName).ToList();
                var assocSpecimen = Context.Specimen.Where(s => assocSiteNames.Contains(s.AssociatedSiteName));

                Context.Specimen.RemoveRange(assocSpecimen);
                Context.Sites.RemoveRange(assocSites);
                Context.Trips.Remove(trip);

                return await Context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteAllAsync()
        {
            return await Task.Run(() =>
            {
                if (Context.Trips.Count() > 0)
                {
                    Context.Trips.ForEach<Trip>(async trip =>
                    {
                        await DeleteAsync(trip);
                    });
                    return true;
                }
                return false;
            });
        }

        public async Task<IEnumerable<Trip>> GetAllAsync(bool forceRefresh = false)
        {
            return await Context.Trips.ToListAsync();
        }

        public async Task<Trip> GetAsync(int id)
        {
            return await Context.Trips.Where(t
                => t.TripID == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Trip trip)
        {
            if (trip is { })
            {
                Context.Trips.Update(trip);
                return await Context.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> GetNextCollectionNumber()
        {
            int tripCount = await Context.Trips.CountAsync();
            return tripCount + 1;
        }

        public async Task<Trip> GetByNameAsync(string name)
        {
            if (!string.IsNullOrEmpty(name))
                return await Context.Trips.Where(t => t.TripName == name).FirstOrDefaultAsync();
            else
                return null;
        }
    }
}