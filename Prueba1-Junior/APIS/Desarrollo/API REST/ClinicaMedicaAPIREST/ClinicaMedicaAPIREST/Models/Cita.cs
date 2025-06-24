namespace ClinicaMedicaAPIREST.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public int Paciente_Id { get; set; }
        public int Medico_Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public bool Estado { get; set; }
    }
}
