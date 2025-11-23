using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MasazeApp.Data
{
    public class AppUser : IdentityUser
    {
        
    }

    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<Masaza> Masaza => Set<Masaza>();
    }

    public class Masaza
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = "";
        public int TrajanjeMin { get; set; }
        public decimal Cena { get; set; }
        public bool Aktivna { get; set; } = true;
    }
}
