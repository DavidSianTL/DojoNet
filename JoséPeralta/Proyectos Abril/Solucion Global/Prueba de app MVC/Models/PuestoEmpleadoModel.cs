using System.ComponentModel.DataAnnotations;
namespace Prueba_de_app_MVC.Models
{
    public class PuestoEmpleadoModel
    {
        public int IdPuesto { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(20, ErrorMessage = "El campo no puede tener más de 20 caracteres")]
        [MinLength(5, ErrorMessage = "El campo debe tener al menos 5 caracteres")]
        [Display(Name = "Nombre del puesto")]
        public string Puesto { get; set; } 

    }
}