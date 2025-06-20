namespace ClinicaApi.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }

}
