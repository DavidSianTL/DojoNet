using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaDeSeguridad.Models;

namespace SistemaDeSeguridad.Data
{
    public class AppDbContext: IdentityDbContext<Usuario, CustomRolUsuario, string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Configuracion de la relacion muchos a muchos entre roles y permisos
            builder.Entity<PermisoRol>().HasKey(rp => new { rp.RolId, rp.PermisoId });

            builder.Entity<PermisoRol>().HasOne(rp => rp.Rol).WithMany(r => r.RolPermisos).HasForeignKey(rp => rp.RolId);

            builder.Entity<PermisoRol>().HasOne(rp => rp.Permisos).WithMany(p => p.RolPermisos).HasForeignKey(rp => rp.PermisoId);

            //Configurar autoincremento para cada modelo
            builder.Entity<Permisos>().Property(p => p.Id).ValueGeneratedOnAdd();
        }

        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<PermisoRol> PermisoRol { get; set; }

    }
}
