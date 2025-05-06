using System.ComponentModel.DataAnnotations;

namespace TiendaEnLinea_MR.Models
{
    public class UsuarioViewModel
    {
        public string Correo { get; set; }
        public string Password { get; set; }
        public string NombreCompleto { get; set; }
    }
}
