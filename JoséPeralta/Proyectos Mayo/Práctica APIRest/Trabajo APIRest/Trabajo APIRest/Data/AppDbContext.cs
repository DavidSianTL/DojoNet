using Microsoft.EntityFrameworkCore;
using Trabajo_APIRest.Models;

namespace Trabajo_APIRest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<EspecialidadViewModel> Especialidades { get; set; }
        public DbSet<MedicoViewModel> Medicos { get; set; }
        public DbSet<MedicoEspecialidad> MedicoEspecialidades { get; set; }
        public DbSet<PacienteViewModel> Pacientes { get; set; }
        public DbSet<CitaViewModel> Citas { get; set; }
        public DbSet<UsuarioViewModel> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la tabla Usuarios
            modelBuilder.Entity<UsuarioViewModel>().ToTable("Usuarios");
            modelBuilder.Entity<UsuarioViewModel>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<UsuarioViewModel>().Property(u => u.IdUsuario).HasColumnName("idUsuario");
            modelBuilder.Entity<UsuarioViewModel>().Property(u => u.NombreCompleto).HasColumnName("nombreCompleto");
            modelBuilder.Entity<UsuarioViewModel>().Property(u => u.Usuario).HasColumnName("usuario");
            modelBuilder.Entity<UsuarioViewModel>().Property(u => u.Contrasenia).HasColumnName("contrasenia");
            modelBuilder.Entity<UsuarioViewModel>().Property(u => u.Token).HasColumnName("token");

            // Configuración de la tabla Especialidades
            modelBuilder.Entity<EspecialidadViewModel>().ToTable("Especialidades");
            modelBuilder.Entity<EspecialidadViewModel>().HasKey(e => e.IdEspecialidad);
            modelBuilder.Entity<EspecialidadViewModel>().Property(e => e.IdEspecialidad).HasColumnName("idEspecialidad");
            modelBuilder.Entity<EspecialidadViewModel>().Property(e => e.Nombre).HasColumnName("nombre").IsRequired();

            // Configuración de la tabla Medicos
            modelBuilder.Entity<MedicoViewModel>().ToTable("Medicos");
            modelBuilder.Entity<MedicoViewModel>().HasKey(m => m.IdMedico);
            modelBuilder.Entity<MedicoViewModel>().Property(m => m.IdMedico).HasColumnName("idMedico");
            modelBuilder.Entity<MedicoViewModel>().Property(m => m.Nombre).HasColumnName("nombre").IsRequired();
            modelBuilder.Entity<MedicoViewModel>().Property(m => m.Email).HasColumnName("email").IsRequired();

            // Configuración de la tabla MedicoEspecialidades
            modelBuilder.Entity<MedicoEspecialidad>().ToTable("MedicoEspecialidades");
            modelBuilder.Entity<MedicoEspecialidad>().HasKey(me => me.IdMedicoEspecialidad);
            modelBuilder.Entity<MedicoEspecialidad>().Property(me => me.IdMedicoEspecialidad).HasColumnName("idMedicoEspecialidad");
            modelBuilder.Entity<MedicoEspecialidad>().Property(me => me.MedicoId).HasColumnName("fk_IdMedico").IsRequired();
            modelBuilder.Entity<MedicoEspecialidad>().Property(me => me.EspecialidadId).HasColumnName("fk_IdEspecialidad").IsRequired();

            // Configuración de las relaciones para MedicoEspecialidades
            modelBuilder.Entity<MedicoEspecialidad>(entity =>
            {
                entity.ToTable("MedicoEspecialidades");
                entity.HasKey(e => e.IdMedicoEspecialidad);
                
                entity.Property(e => e.IdMedicoEspecialidad)
                    .HasColumnName("idMedicoEspecialidad");
                    
                entity.Property(e => e.MedicoId)
                    .HasColumnName("fk_IdMedico");
                    
                entity.Property(e => e.EspecialidadId)
                    .HasColumnName("fk_IdEspecialidad");
                    
                entity.HasOne(me => me.Medico)
                    .WithMany(m => m.MedicoEspecialidades)
                    .HasForeignKey(me => me.MedicoId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(me => me.Especialidad)
                    .WithMany(e => e.MedicoEspecialidades)
                    .HasForeignKey(me => me.EspecialidadId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de la tabla Pacientes
            modelBuilder.Entity<PacienteViewModel>().ToTable("Pacientes");
            modelBuilder.Entity<PacienteViewModel>().HasKey(p => p.IdPaciente);
            modelBuilder.Entity<PacienteViewModel>().Property(p => p.IdPaciente).HasColumnName("idPaciente");
            modelBuilder.Entity<PacienteViewModel>().Property(p => p.Nombre).HasColumnName("nombre").IsRequired();
            modelBuilder.Entity<PacienteViewModel>().Property(p => p.Email).HasColumnName("email").IsRequired();
            modelBuilder.Entity<PacienteViewModel>().Property(p => p.Telefono).HasColumnName("telefono");
            modelBuilder.Entity<PacienteViewModel>().Property(p => p.FechaNacimiento).HasColumnName("fechaNacimiento").IsRequired();

            // Configuración de la tabla Citas
            modelBuilder.Entity<CitaViewModel>().ToTable("Citas");
            modelBuilder.Entity<CitaViewModel>().HasKey(c => c.IdCita);
            modelBuilder.Entity<CitaViewModel>().Property(c => c.IdCita).HasColumnName("idCita");
            modelBuilder.Entity<CitaViewModel>().Property(c => c.Fk_IdPaciente).HasColumnName("fk_IdPaciente").IsRequired();
            modelBuilder.Entity<CitaViewModel>().Property(c => c.Fk_IdMedico).HasColumnName("fk_IdMedico").IsRequired();
            modelBuilder.Entity<CitaViewModel>().Property(c => c.Fecha).HasColumnName("fecha").IsRequired();
            modelBuilder.Entity<CitaViewModel>().Property(c => c.Hora).HasColumnName("hora").IsRequired();

            // Configuración de las relaciones para Citas
            modelBuilder.Entity<CitaViewModel>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)  // Asegúrate de que PacienteViewModel tenga esta propiedad
                .HasForeignKey(c => c.Fk_IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CitaViewModel>()
                .HasOne(c => c.Medico)
                .WithMany(m => m.Citas)  // Asegúrate de que MedicoViewModel tenga esta propiedad
                .HasForeignKey(c => c.Fk_IdMedico)
                .OnDelete(DeleteBehavior.Restrict);
            }
    }
}