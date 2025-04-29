using System.ComponentModel.DataAnnotations;

namespace Examen_mes_abril.Models
{
    public class UsuarioViewModel
    {
        [EmailAddress]
        public string Correo { get; set; }
        public string Password { get; set; }

    }
}
