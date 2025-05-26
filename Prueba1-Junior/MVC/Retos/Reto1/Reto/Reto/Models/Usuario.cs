using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Reto.Models
{
    public class Usuario
    {
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
