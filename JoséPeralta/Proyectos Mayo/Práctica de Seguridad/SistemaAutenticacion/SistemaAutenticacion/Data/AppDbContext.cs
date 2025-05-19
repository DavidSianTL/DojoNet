using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Data
{
    public class AppDbContext : IdentityDbContext<UsuarioViewModel, CustomRolUsuarioViewModel, string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración adicional
            // Relación muchos a muchos entre Roles y Permisos
            builder.Entity<PermisosRolViewModel>().HasKey(rp => new { rp.RoldId, rp.PermisoId });

            builder.Entity<PermisosRolViewModel>().HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.RoldId);

            builder.Entity<PermisosRolViewModel>().HasOne(rp => rp.Permiso)
                .WithMany(p. => p.PermisosRoles)
                .HasForeignKey(rp => rp.PermisoId);

            // Autoincremento de la llave primaria
            builder.Entity<PermisosViewModel>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

        }





    }
}
