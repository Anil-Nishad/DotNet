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
