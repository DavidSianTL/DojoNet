using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trabajo_APIRest.Models
{
    [Table("Pacientes")]
    public class PacienteViewModel
    {
        [Key]
        [Column("idPaciente")]
        public int IdPaciente { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("telefono")]
        public string Telefono { get; set; }

        [Column("fechaNacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; } = DateTime.UtcNow;

        // Relación: Un paciente puede tener muchas citas
        public ICollection<CitaViewModel>? Citas { get; set; }
    }
}
