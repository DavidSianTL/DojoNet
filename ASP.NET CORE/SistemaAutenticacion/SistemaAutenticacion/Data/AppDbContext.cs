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

            //Configuracion de la relacion muchos a muchos entre roles y permisos
            builder.Entity<PermisoRol>().HasKey(rp => new { rp.RolId, rp.PermisoId });

            builder.Entity<PermisoRol>().HasOne(rp => rp.Rol).WithMany(r => r.RolPermisos).HasForeignKey(rp => rp.RolId);

            builder.Entity<PermisoRol>().HasOne(rp => rp.Permisos).WithMany(p => p.RolPermisos).HasForeignKey(rp => rp.PermisoId);

            //configurar autoincremento para cada modelo
            builder.Entity<Permisos>().Property(p => p.Id).ValueGeneratedOnAdd();
        }

        //1. Modelo - nombre que tendra en la DB
        //2. Las nuevas clases creadas se debera colocar aqui para poder migrar
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<PermisoRol> PermisoRol { get; set; }

    }
}
