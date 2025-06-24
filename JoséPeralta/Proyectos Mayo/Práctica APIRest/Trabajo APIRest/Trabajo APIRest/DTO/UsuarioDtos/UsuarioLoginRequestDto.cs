using System.ComponentModel.DataAnnotations;

namespace Trabajo_APIRest.Dtos.UsuarioDtos
{
   
    public class UsuarioLoginRequestDto
    {
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
    }
}
