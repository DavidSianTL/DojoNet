using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Trabajo_APIRest.Models
{
    [Table("MedicoEspecialidades")]
public class MedicoEspecialidad
{
    [Key]
    [Column("idMedicoEspecialidad")]
    public int IdMedicoEspecialidad { get; set; }

    [Column("fk_IdMedico")]
    public int MedicoId { get; set; }

    [Column("fk_IdEspecialidad")]
    public int EspecialidadId { get; set; }

    [JsonIgnore]
    public virtual MedicoViewModel? Medico { get; set; }

    [JsonIgnore]
    public virtual EspecialidadViewModel? Especialidad { get; set; }
}
}
