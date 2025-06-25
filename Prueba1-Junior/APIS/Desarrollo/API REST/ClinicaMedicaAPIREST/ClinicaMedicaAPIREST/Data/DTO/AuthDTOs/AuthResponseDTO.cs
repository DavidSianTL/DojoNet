namespace ClinicaMedicaAPIREST.Data.DTO.AuthDTOs
{
    public class AuthResponseDTO
    {
        public string Username { get; set; } = null!;
        public string Role {  get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime TokenExpiration { get; set; }
    }
}
