using System.ComponentModel.DataAnnotations;

namespace MiBanco.Dto.Login.Request
{
    public class LoginRequest
    {
        [Required]
        public string Usuario { get; set; }

        [Required]
        public string Contraseña { get; set; }
    }
}
