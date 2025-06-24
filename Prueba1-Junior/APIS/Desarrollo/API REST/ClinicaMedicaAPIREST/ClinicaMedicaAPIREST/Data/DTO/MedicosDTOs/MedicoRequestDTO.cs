namespace ClinicaMedicaAPIREST.Data.DTO.MedicosDTOs
{
    public class MedicoRequestDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Especialidad { get; set; }

    }
}
