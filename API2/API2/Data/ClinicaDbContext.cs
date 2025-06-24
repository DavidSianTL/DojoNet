using API2.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using API2.Models;
using Microsoft.EntityFrameworkCore;
using API2.Models;
using API2.Models;

namespace API2.Data
{
    public class ClinicaDbContext : DbContext
    {
        public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options)
            : base(options)
        {
        }

        // DbSets para cada entidad
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación Paciente-Cita
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(c => c.Paciente_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Medico-Cita
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Medico)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.Medico_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Medico-Especialidad
            modelBuilder.Entity<Medico>()
                .HasOne(m => m.Especialidad)
                .WithMany(e => e.Medicos)
                .HasForeignKey(m => m.Especialidad_Id)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
