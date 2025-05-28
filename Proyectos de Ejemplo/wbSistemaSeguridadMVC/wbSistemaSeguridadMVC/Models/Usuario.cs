
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace wbSistemaSeguridadMVC.Models
{
    public class Usuario
    {

        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El login de usuario es obligatorio.")]
        public string UsuarioLg { get; set; }
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string NomUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contrasenia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        public int FkIdEstado { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedadde navegación para el estado del usuario
        [ValidateNever]
        public Estado_Usuario Estado { get; set; }  //inicializarlo
    }
}
