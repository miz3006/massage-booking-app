using MassageStudio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MassageStudio.Data
{
    public class MassageContext : IdentityDbContext<ApplicationUser>
    {
        public MassageContext(DbContextOptions<MassageContext> options)
            : base(options)
        {
        }

        public DbSet<Client>  Clients  { get; set; } = default!;
        public DbSet<Service> Services { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // NUJNO za Identity

            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Service>().ToTable("Service");
        }
    }
}
