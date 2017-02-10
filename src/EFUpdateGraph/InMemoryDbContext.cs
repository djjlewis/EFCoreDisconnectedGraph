using Microsoft.EntityFrameworkCore;

namespace EFUpdateGraph
{
    public class InMemoryDbContext : DbContext
    {
        public DbSet<Floor> Floors { get; set; }

        public InMemoryDbContext()
        {
            
        }

        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
        {
            
        }

    }
}