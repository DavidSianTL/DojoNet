using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Dtos.Empleados.Requests
{
    public class CrearEmpleadoRequest
    {

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Pais { get; set; } = string.Empty;

        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo {0} debe tener exactamente {1} dígitos.")]
        [RegularExpression(@"^(\d{13})?$", ErrorMessage = "El campo {0} debe contener exactamente 13 números.")]
        public string? DPI { get; set; }

        [StringLength(15, MinimumLength = 15, ErrorMessage = "El campo {0} debe tener exactamente {1} dígitos.")]
        [RegularExpression(@"^([a-zA-Z0-9]{15})?$", ErrorMessage = "El campo {0} debe contener exactamente 15 caracteres alfanuméricos.")]
        public string? Pasaporte { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        public string NombresEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        public string ApellidosEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido.")]
        public string CorreoPersonal { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres")]
        [RegularExpression(@"^[0-9+\-\(\)\s]+$", ErrorMessage = "Solo se permiten números, espacios, guiones y paréntesis")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido.")]
        [StringLength(50)]
        public string CorreoInstitucional { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0.01, 100000.00, ErrorMessage = "El campo {0} debe ser un número positivo válido.")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.DateTime)]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        public int Estado { get; set; }

    }
}
