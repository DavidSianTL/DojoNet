namespace ClinicaMedicaAPIREST.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Especialidad { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
