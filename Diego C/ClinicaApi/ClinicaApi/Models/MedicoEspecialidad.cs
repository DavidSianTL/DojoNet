using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicaApi.Models
{
    public class MedicoEspecialidad
    {
        public int MedicoId { get; set; }

        [ForeignKey("MedicoId")]
        [JsonIgnore]
        public virtual Medico? Medico { get; set; }

        public int EspecialidadId { get; set; }

        [ForeignKey("EspecialidadId")]
        [JsonIgnore]
        public virtual Especialidad? Especialidad { get; set; }
    }
}