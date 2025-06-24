using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; 

namespace ApiClinicaMedica.Models
{
    public class Cita
    {
        [Key]
        public int IdCita { get; set; }

        public int PacienteId { get; set; }

        [JsonIgnore]  
        public Paciente? Paciente { get; set; }

        public int MedicoId { get; set; }

        [JsonIgnore]
        public Medico? Medico { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
