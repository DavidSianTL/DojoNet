using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using API2.Models;

namespace API2.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        // Relación con Paciente
        
        public int Paciente_Id { get; set; }

        [JsonIgnore]
        public Paciente? Paciente { get; set; }

        // Relación con Medico

        public int Medico_Id { get; set; }

        [JsonIgnore]
        [ForeignKey("Medico_Id")]
        public Medico? Medico { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }
      
    }
}
