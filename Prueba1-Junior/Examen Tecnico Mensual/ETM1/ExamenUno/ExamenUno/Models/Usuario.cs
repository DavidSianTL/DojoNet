using System.ComponentModel.DataAnnotations;

namespace ExamenUno.Models
{
    public class Usuario
    {
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        public string password {  get; set; } = string.Empty;
    }
}
