using Microsoft.EntityFrameworkCore;
using VittorioApiT2M.Domain.Entities;

namespace VittorioApiT2M.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Cliente> Usuarios { get; set; }
    }
}
