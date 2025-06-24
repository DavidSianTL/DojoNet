using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trabajo_APIRest.Models
{
    [Table("Especialidades")]
    public class EspecialidadViewModel
    {
        [Key]
        [Column("idEspecialidad")]
        public int IdEspecialidad { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        // Relación: Una especialidad puede tener muchos médicos
        [JsonIgnore] // Evita la serialización circular
        public ICollection<MedicoViewModel>? Medicos { get; set; }
    }
}
