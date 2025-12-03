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

        public DbSet<Masaza> Masaze { get; set; }  
    }
}
