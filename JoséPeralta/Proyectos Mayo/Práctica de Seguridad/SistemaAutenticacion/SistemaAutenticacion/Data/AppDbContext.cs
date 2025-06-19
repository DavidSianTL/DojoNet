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

            // Configuración
            builder.Entity<CategoriasViewModel>().HasKey(c => c.IdCategoria); // Llave primaria

            builder.Entity<ProductosViewModel>().HasKey(p => p.IdProducto); // Llave primaria
            builder.Entity<ProductosViewModel>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2); // 18 dígitos en total, 2 decimales
            builder.Entity<ProductosViewModel>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.IdCategoria);


            // Configuración adicional
            // Relación muchos a muchos entre Roles y Permisos
            builder.Entity<PermisosRolViewModel>().HasKey(rp => new { rp.RolId, rp.PermisoId });

            builder.Entity<PermisosRolViewModel>().HasOne(rp => rp.Rol).WithMany(r => r.RolPermisos).HasForeignKey(rp => rp.RolId);

            builder.Entity<PermisosRolViewModel>().HasOne(rp => rp.Permiso).WithMany(p => p.PermisosRoles).HasForeignKey(rp => rp.PermisoId);

            // Autoincremento de la llave primaria
            builder.Entity<PermisosViewModel>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

        }

        // 1. Modelo - Nombre de la tabla que se va a crear en la base de datos
        public DbSet<PermisosViewModel> Permisos { get; set; } // Tabla de permisos
        public DbSet<PermisosRolViewModel> PermisosRol { get; set; } // Tabla de permisos por rol



    }
}
