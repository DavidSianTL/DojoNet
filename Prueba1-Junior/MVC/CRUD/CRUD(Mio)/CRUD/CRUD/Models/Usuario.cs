using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class Usuario
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
