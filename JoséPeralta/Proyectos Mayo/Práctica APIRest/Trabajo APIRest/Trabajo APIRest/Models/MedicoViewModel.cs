using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;

namespace Trabajo_APIRest.Models
{
    [Table("Medicos")]
    public class MedicoViewModel
    {
        [Key]
        [Column("idMedico")]
        public int IdMedico { get; set; }

        [Column("nombre")]
        [Required]
        public string Nombre { get; set; }

        [Column("email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        // Propiedad de navegación para la relación muchos a muchos
        [JsonIgnore]
        public virtual ICollection<MedicoEspecialidad> MedicoEspecialidades { get; set; } = new List<MedicoEspecialidad>();
        
        // Propiedad de conveniencia para acceder a las especialidades directamente
        [NotMapped]
        [JsonIgnore]
        public List<EspecialidadViewModel> Especialidades 
        { 
            get { return MedicoEspecialidades?.Select(me => me.Especialidad).ToList() ?? new List<EspecialidadViewModel>(); } 
        }

        // Relación: Un médico puede tener muchas citas
        [JsonIgnore] // Evita la serialización circular
    public virtual ICollection<CitaViewModel> Citas { get; set; } = new List<CitaViewModel>();
    }
}
