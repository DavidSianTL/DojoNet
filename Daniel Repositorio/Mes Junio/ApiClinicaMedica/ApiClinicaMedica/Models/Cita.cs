using System.ComponentModel.DataAnnotations;

namespace ApiClinicaMedica.Models
{
    public class Cita
    {
        [Key]
        public int IdCita { get; set; }
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
