namespace ClinicaMedicaAPIREST.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono {  get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public bool Estado { get; set; }
    }
}
