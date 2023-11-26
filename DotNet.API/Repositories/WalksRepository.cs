using DotNet.API.Data;
using DotNet.API.Models.Domain;
using DotNet.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DotNet.API.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext dbContext;

        public WalksRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return new Walk();
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            //To Return only Walks
            //return await dbContext.Walks.ToListAsync();

            //To Return Walks + Navigation Property
            //return await dbContext.Walks.Include("Diffculty").Include("Region").ToListAsync();

            //To Return Walks + Navigation Property with TypeSafe
            return await dbContext.Walks
                .Include(x => x.Difficulty)
                .Include(x => x.Region)
                .ToListAsync();
        }

        public async Task<List<Walk>> GetAllAsyncFSP(string? filterOn = null,
                                                     string? filterQuery = null,
                                                     string? sortBy = null,
                                                     bool isAscending = true,
                                                     int pageNumber = 1,
                                                     int pageSize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrEmpty(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    //walks = walks.Where(x => x.Name == filterQuery);
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting 
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

            //return await walks.ToListAsync();
        }

        public async Task<Walk?> GetAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Diffculty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk == null)
            {
                return null;
            }
          
            existingWalk.Name = walk.Name;
            existingWalk.Region = walk.Region;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.Difficulty = walk.Difficulty;

            await dbContext.SaveChangesAsync();

            return existingWalk;
           
        }
    }
}
