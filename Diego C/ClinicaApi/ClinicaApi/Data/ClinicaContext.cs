using Microsoft.EntityFrameworkCore;
using ClinicaApi.Models;

namespace ClinicaApi.Data
{
    public class ClinicaContext : DbContext
    {
        public ClinicaContext(DbContextOptions<ClinicaContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<MedicoEspecialidad> MedicoEspecialidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MedicoEspecialidad>()
                .ToTable("MedicoEspecialidad");

            modelBuilder.Entity<MedicoEspecialidad>()
                .HasKey(me => new { me.MedicoId, me.EspecialidadId });

            modelBuilder.Entity<MedicoEspecialidad>()
                .HasOne(me => me.Medico)
                .WithMany(m => m.MedicoEspecialidades)
                .HasForeignKey(me => me.MedicoId);

            modelBuilder.Entity<MedicoEspecialidad>()
                .HasOne(me => me.Especialidad)
                .WithMany(e => e.MedicoEspecialidades)
                .HasForeignKey(me => me.EspecialidadId);
        }
    }
}
