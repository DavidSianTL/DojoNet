using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Dtos.Empleados.Requests
{
    public class ActualizarEmpleadoRequest
    {
        [Required(ErrorMessage = "El ID del empleado es obligatorio.")]
        public int IdEmpleado { get; set; }

        [StringLength(15, MinimumLength = 15, ErrorMessage = "El campo {0} debe tener exactamente {1} dígitos.")]
        [RegularExpression(@"^\d{15}$", ErrorMessage = "El campo {0} debe contener exactamente 15 números.")]
        public string? Pasaporte { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        public string NombresEmpleado { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        public string ApellidosEmpleado { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido.")]
        [StringLength(50)]
        public string CorreoInstitucional { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido.")]
        [StringLength(50)]
        public string CorreoPersonal { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres")]
        [RegularExpression(@"^[0-9+\-\(\)\s]+$", ErrorMessage = "Solo se permiten números, espacios, guiones y paréntesis")]
        public string Telefono { get; set; }

        [StringLength(15, MinimumLength = 7, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^\d{7,15}-?\d{0,1}$", ErrorMessage = "El campo {0} debe tener formato numérico válido.")]
        public string NIT { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "El campo {0} no debe exceder los {1} caracteres.")]
        [RegularExpression(@"^(Masculino|Femenino|Otro)$", ErrorMessage = "El campo {0} debe ser 'Masculino', 'Femenino' u 'Otro'.")]
        public string Genero { get; set; } = string.Empty;

        [Range(0.01, 100000.00, ErrorMessage = "El campo {0} debe ser un número positivo válido.")]
        public decimal Salario { get; set; }

        public int Estado { get; set; }
    }
}
