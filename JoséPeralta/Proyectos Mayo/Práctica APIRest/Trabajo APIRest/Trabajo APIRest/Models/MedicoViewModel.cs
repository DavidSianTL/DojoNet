using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trabajo_APIRest.Models
{
    [Table("Medicos")]
    public class MedicoViewModel
    {
        [Key]
        [Column("idMedico")]
        public int IdMedico { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("fk_IdEspecialidad")]
        public int? Fk_IdEspecialidad { get; set; }

        [Column("email")]
        public string Email { get; set; }

        // Relación: Un médico pertenece a una especialidad
        [JsonIgnore] // Evita la serialización circular
        [ForeignKey("Fk_IdEspecialidad")]
        public EspecialidadViewModel? Especialidad { get; set; }

        // Relación: Un médico puede tener muchas citas
        [JsonIgnore] // Evita la serialización circular
        public ICollection<CitaViewModel>? Citas { get; set; }
    }
}
