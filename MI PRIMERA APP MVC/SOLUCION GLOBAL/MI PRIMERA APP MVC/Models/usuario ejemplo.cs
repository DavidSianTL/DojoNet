using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Antiforgery;

namespace MI_PRIMERA_APP_MVC.Models
{
    public class usuario_ejemplo
    {
        [Required(ErrorMessage= "El campo Nombre es obligatorio")]
        [StringLengthAttribute(50, ErrorMessage = "El campo nombre no puede tener mas de 50 caracteres")]
        [MinLength(3, ErrorMessage = "El campo Nombre no puede tener menos de 3 caracteres")]
        [Display(Name ="Nombre Completo")]
        public string Nombre { get; set; }

        public string Correo { get; set; }
        public string Password { get; set; }

    }
}
