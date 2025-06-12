namespace CRUD_Usuarios_simple.Models;
using System.ComponentModel.DataAnnotations;

public class EstadoViewModel
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
    [Display(Name = "Nombre del Estado")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El campo {0} solo puede contener letras.")]
    public string NombreEstado { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;
}