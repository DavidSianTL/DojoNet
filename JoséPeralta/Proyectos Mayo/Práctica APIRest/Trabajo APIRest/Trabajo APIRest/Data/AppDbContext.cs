using Microsoft.EntityFrameworkCore;
using Trabajo_APIRest.Models;


namespace Trabajo_APIRest.Data
{
    public class AppDbContext: DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<EspecialidadViewModel> Especialidades { get; set; }
        public DbSet<MedicoViewModel> Medicos { get; set; }
        public DbSet<PacienteViewModel> Pacientes { get; set; }
        public DbSet<CitaViewModel> Citas { get; set; }
        public DbSet<UsuarioViewModel> Usuarios { get; set; }

        // OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llama al método base para que se creen las tablas por defecto
            base.OnModelCreating(modelBuilder);

            // Configuración de la tabla Usuarios
            modelBuilder.Entity<UsuarioViewModel>().ToTable("Usuarios");
            modelBuilder.Entity<UsuarioViewModel>().HasKey(u => u.IdUsuario);

            // Configuración de la tabla Especialidades
            modelBuilder.Entity<EspecialidadViewModel>().ToTable("Especialidades");
            modelBuilder.Entity<EspecialidadViewModel>().HasKey(e => e.IdEspecialidad);

            // Configuración de la tabla Medicos
            modelBuilder.Entity<MedicoViewModel>()
            .ToTable("Medicos")
            .HasKey(m => m.IdMedico);

            modelBuilder.Entity<MedicoViewModel>()
                .Property(m => m.Fk_IdEspecialidad)
                .HasColumnName("fk_IdEspecialidad");
            modelBuilder.Entity<MedicoViewModel>()
                .Property(m => m.Nombre)
                .HasColumnName("nombre");
            modelBuilder.Entity<MedicoViewModel>()
                .Property(m => m.Email)
                .HasColumnName("email");
            modelBuilder.Entity<MedicoViewModel>()
                .Property(m => m.IdMedico)
                .HasColumnName("idMedico");

            modelBuilder.Entity<MedicoViewModel>()
                .HasOne(m => m.Especialidad)
                .WithMany(e => e.Medicos)  // o .WithMany() si no defines Medicos en EspecialidadViewModel
                .HasForeignKey(m => m.Fk_IdEspecialidad)
                .HasConstraintName("FK_Medico_Especialidad");


            // Configuración de la tabla Pacientes
            modelBuilder.Entity<PacienteViewModel>().ToTable("Pacientes");
            modelBuilder.Entity<PacienteViewModel>().HasKey(p => p.IdPaciente);

            // Configuración de la tabla Citas
            modelBuilder.Entity<CitaViewModel>().ToTable("Citas");
            modelBuilder.Entity<CitaViewModel>().HasKey(c => c.IdCita);

        }

    }
}
