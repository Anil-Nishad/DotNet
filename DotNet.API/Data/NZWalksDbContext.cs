using DotNet.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotNet.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        // To create Constructor
        //ctor + double Tab
        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
