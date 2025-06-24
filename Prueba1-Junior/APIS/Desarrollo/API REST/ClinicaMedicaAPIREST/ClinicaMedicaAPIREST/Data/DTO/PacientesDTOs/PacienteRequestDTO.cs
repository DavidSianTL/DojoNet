namespace ClinicaMedicaAPIREST.Data.DTO.PacientesDTOs
{
    public class PacienteRequestDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
    }
}
