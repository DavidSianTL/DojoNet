using System.ComponentModel.DataAnnotations;

namespace MiPrimeraAppMVC.Models
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "El campo Id es requerido")]
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(50, ErrorMessage = "El campo Nombre no puede tener más de 50 caracteres")]

        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El campo Apellido es requerido")]
        [StringLength(50, ErrorMessage = "El campo Apellido no puede tener más de 50 caracteres")]
        public string? Apellido { get; set; }
        [Required(ErrorMessage = "El campo Nombre de Usuario es requerido")]
        [StringLength(15, ErrorMessage = "El campo Nombre de Usuario no puede tener más de 15 caracteres")]
        [Display(Name = "Nombre de Usuario")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido")]
        [StringLength(15, ErrorMessage = "El campo Contraseña no puede tener más de 15 caracteres")]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "El campo Correo Electrónico es requerido")]
        [StringLength(100, ErrorMessage = "El campo Correo Electrónico no puede tener más de 100 caracteres")]
        [EmailAddress(ErrorMessage = "El campo Correo Electrónico no es una dirección de correo electrónico válida")]
        [Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        public DateTime FechaIngreso { get; set; }
        public int Status { get; set; }
    }
}

