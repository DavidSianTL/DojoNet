using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Models;

namespace SistemaAutenticacion.Data
{
    /// <summary>
    /// Esta clase nos permite gestionar las migraciones a la base de datos
    /// </summary>
    public class AppDbContext: IdentityDbContext<Usuarios, CustomRolUsuario, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)        
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

            //Configuracion de la relacion muchos a muchos entre roles y permisos
            builder.Entity<PermisoRol>().HasKey(rp => new { rp.RolId, rp.PermisoId });

            builder.Entity<PermisoRol>().HasOne(rp => rp.Rol).WithMany(r => r.RolPermisos).HasForeignKey(rp => rp.RolId);

            builder.Entity<PermisoRol>().HasOne(rp => rp.Permisos).WithMany(p => p.RolPermisos).HasForeignKey(rp => rp.PermisoId);

            //configurar autoincremento para cada modelo
            builder.Entity<Permiso>().Property(p => p.Id).ValueGeneratedOnAdd();
        }

        //1. Modelo - nombre que tendra en la DB
        //2. Las nuevas clases creadas se debera colocar aqui para poder migrar
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<PermisoRol> PermisoRol { get; set; }

    }
}
