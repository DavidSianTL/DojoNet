using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;


namespace UsuariosApi.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<UsuarioEF> Usuarios { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Auditoria>().ToTable("Auditoria");
            modelBuilder.Entity<UsuarioEF>().ToTable("Usuarios");
            modelBuilder.Entity<UsuarioEF>().HasKey(u => u.IdUsuario);


        }

    }
}
