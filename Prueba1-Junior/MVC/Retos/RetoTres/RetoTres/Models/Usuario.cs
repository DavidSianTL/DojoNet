using System.ComponentModel.DataAnnotations;

namespace RetoTres.Models
{
    public class Usuario
    {
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Password { get; set;}
    }
}
