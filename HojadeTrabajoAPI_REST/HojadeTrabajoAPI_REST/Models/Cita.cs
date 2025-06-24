namespace HojadeTrabajoAPI_REST.Models
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int FK_Id_Paciente { get; set; }
        public int FK_Id_Medico { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
