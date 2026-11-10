using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task8.Models;
using Task8.Models;

namespace task8.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {

        public DbSet<Product> products { get; set; }

        public DbSet<SupplyRequest> supplyRequests { get; set; }

        public DbSet<RequestItem> requestItems { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SupplyRequest>()
                .Property(o => o.Status)
                .HasConversion<string>();
        }
    }
}