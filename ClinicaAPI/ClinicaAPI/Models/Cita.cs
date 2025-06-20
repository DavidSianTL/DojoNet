using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicaAPI.Models;

namespace ClinicaAPI.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Paciente")]
        public int Paciente_id { get; set; }

        [Required]
        [ForeignKey("Medico")]
        public int Medico_id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        // Propiedades de navegación
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
    }
}
