using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trabajo_APIRest.Models
{
    [Table("Citas")]
    public class CitaViewModel
    {
        [Key]
        [Column("idCita")]
        public int IdCita { get; set; }

        [Column("fk_IdPaciente")]
        public int Fk_IdPaciente { get; set; }

        [Column("fk_IdMedico")]
        public int Fk_IdMedico { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("hora")]
        public TimeSpan Hora { get; set; }

        // Relaciones de navegación
        [JsonIgnore] // Evita la serialización circular
        [ForeignKey("Fk_IdPaciente")]
        public PacienteViewModel? Paciente { get; set; }

        [JsonIgnore] // Evita la serialización circular
        [ForeignKey("Fk_IdMedico")]
        public MedicoViewModel? Medico { get; set; }
    }
}
