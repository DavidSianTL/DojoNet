using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using API2.Models;
using API2.Models;

namespace API2.Models
{
    public class Medico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        // Relación con Especialidad
        [JsonIgnore]
        public int? Especialidad_Id { get; set; }
        [JsonIgnore]
        [ForeignKey("Especialidad_Id")]
        public Especialidad? Especialidad { get; set; }

        // Relación con Citas
        [JsonIgnore]
        public ICollection<Cita>? Citas { get; set; }
    }
}
