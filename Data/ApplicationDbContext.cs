using CartaDeclaratoriaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CartaDeclaratoriaApp.Data
{
    // Hereda de IdentityDbContext para incluir login/usuarios internos automáticamente
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CartaDeclaratoria> CartasDeclaratorias { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CartaDeclaratoria>(entity =>
            {
                entity.Property(c => c.Monto).HasColumnType("decimal(18,2)");
                entity.HasIndex(c => c.RemesaFolio);
            });
        }
    }
}
