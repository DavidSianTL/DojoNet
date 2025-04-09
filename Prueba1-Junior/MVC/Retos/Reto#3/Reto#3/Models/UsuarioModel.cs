using System.ComponentModel.DataAnnotations;

namespace Reto_3.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "El campo nombre de usuario es requerido")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo contraseña es requerido")]
        public string Password { get; set; }
    }
}
