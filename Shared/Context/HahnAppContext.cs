using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shared.Context
{
    public class HahnAppContext(DbContextOptions<HahnAppContext> options) : DbContext(options)
    {
        // EF Core Map DogFacts entity
        public DbSet<DogFact> DogFacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
