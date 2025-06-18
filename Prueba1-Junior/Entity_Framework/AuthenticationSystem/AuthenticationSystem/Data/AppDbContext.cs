using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthenticationSystem.Models;

namespace AuthenticationSystem.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, Rol, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        {
            
        }


        // clases nuevas para migración (no heredan de ninguna clase de Identity)
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<PermisoRol> PermisosRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de la relación muchos a muchos entre roles y permisos
            builder.Entity<PermisoRol>().HasKey(pr => new {pr.IdRol, pr.IdPermiso});

            builder.Entity<PermisoRol>().HasOne(pr => pr.rol).WithMany(r => r.permisosRol).HasForeignKey(pr => pr.IdRol);
            builder.Entity<PermisoRol>().HasOne(pr => pr.permiso).WithMany(p => p.permisosRol).HasForeignKey(pr => pr.IdPermiso);

            // Configurar el autoincremento para cada modelo creado (sólo permiso)
            builder.Entity<Permiso>().Property(prt => prt.Id).ValueGeneratedOnAdd();

        }


    }
}
