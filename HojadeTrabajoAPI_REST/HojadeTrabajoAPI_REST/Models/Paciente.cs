namespace HojadeTrabajoAPI_REST.Models
{
    public class Paciente
    {
        public int IdPaciente { get; set; }
        public string Nombre{ get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
