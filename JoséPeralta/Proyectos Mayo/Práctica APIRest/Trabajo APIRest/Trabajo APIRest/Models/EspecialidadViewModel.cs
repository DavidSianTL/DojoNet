using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;

namespace Trabajo_APIRest.Models
{
    [Table("Especialidades")]
    public class EspecialidadViewModel
    {
        [Key]
        [Column("idEspecialidad")]
        public int IdEspecialidad { get; set; }

        [Column("nombre")]
        [Required]
        public string Nombre { get; set; }

        // Propiedad de navegación
        [JsonIgnore]
        public virtual ICollection<MedicoEspecialidad> MedicoEspecialidades { get; set; } = new List<MedicoEspecialidad>();

    }
}
