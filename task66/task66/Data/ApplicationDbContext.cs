
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using task66.Models;

namespace task66.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
