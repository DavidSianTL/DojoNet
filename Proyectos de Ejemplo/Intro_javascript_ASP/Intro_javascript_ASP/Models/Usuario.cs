using System.ComponentModel.DataAnnotations;

namespace Intro_javascript_ASP.Models
{
    public class Usuario
    {
        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Apellido { get; set; }

        [Required]
        public string? Edad { get; set; }
    }
}
