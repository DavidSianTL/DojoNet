using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solucion_Reto_3_MVC.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Apellido { get; set; }
        public string? NombreCompleto => $"{Nombre} {Apellido}";

        [StringLength(8, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El campo {0} debe contener exactamente 8 dígitos.")]
        public int? Telefono { get; set; }
        public DateTime? FechaCreacion { get; set; }

    }

    public class UsuarioServicio
    {
        private readonly string _ArchivoUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "user.json");

        public Usuario ObtenerUsuarios(string userName, string password)
        {
            var users = JsonConvert.DeserializeObject<List<Usuario>>(File.ReadAllText(_ArchivoUsuarios));
            return users?.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }

    }
}
