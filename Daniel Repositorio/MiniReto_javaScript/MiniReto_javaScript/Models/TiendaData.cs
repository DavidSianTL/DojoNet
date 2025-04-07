using System.ComponentModel.DataAnnotations;
namespace MiniReto_javaScript.Models
{
    public class FormularioTienda
    {
        [Required(ErrorMessage = "El campo correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo debe ser válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo contraseña es obligatorio.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contra { get; set; }

        [Required(ErrorMessage = "El campo nombre solo debe contener letras.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El nombre solo puede contener letras.")]
        public string NombreProducto { get; set; }
    }
}
