using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Models
{
    [Table("Empleados")]
    public class EmpleadoViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEmpleado")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string TipoContrato { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Column("Pais")]
        public string Pais { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        [Column("Departamento")]
        public string Departamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        [Column("Municipio")]
        public string Municipio { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(255, MinimumLength = 25, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        [Column("Direccion")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Column("Puesto")]
        public string Puesto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Column("Codigo")]
        public string CodigoEmpleado { get; set; } = string.Empty;

        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo {0} debe tener exactamente {1} dígitos.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El campo {0} debe contener exactamente 13 números.")]
        [Column("DPI")]
        public string? DPI { get; set; } = string.Empty;

        [StringLength(15, MinimumLength = 15, ErrorMessage = "El campo {0} debe tener exactamente {1} dígitos.")]
        [RegularExpression(@"^\d{15}$", ErrorMessage = "El campo {0} debe contener exactamente 15 números.")]
        [Column("Pasaporte")]
        public string? Pasaporte { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        [Column("NombreEmpleado")]
        public string NombresEmpleado { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "El campo {0} solo debe contener letras y espacios.")]
        [Column("ApellidoEmpleado")]
        public string ApellidosEmpleado { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido.")]
        [StringLength(50)]
        [Column("CorreoPersonal")]
        public string CorreoPersonal { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El campo {0} no tiene un formato válido.")]
        [StringLength(50)]
        [Column("CorreoInstitucional")]
        public string CorreoInstitucional { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Column("FechaIngreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;

        [Range(0.00, 365.00, ErrorMessage = "El campo {0} debe estar entre {1} y {2} días.")]
        [Column("DiasVacacionesAcumulados")]
        public decimal DiasVacacionesAcumulados { get; set; } = 0.00M;

        [DataType(DataType.Date)]
        [Column("FechaNacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres")]
        [RegularExpression(@"^[0-9+\-\(\)\s]+$", ErrorMessage = "Solo se permiten números, espacios, guiones y paréntesis")]
        [Column("Telefono")]
        public string Telefono { get; set; }

        [StringLength(15, MinimumLength = 7, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^\d{7,15}-?\d{0,1}$", ErrorMessage = "El campo {0} debe tener formato numérico válido.")]
        [Column("NIT")]
        public string NIT { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "El campo {0} no debe exceder los {1} caracteres.")]
        [RegularExpression(@"^(Masculino|Femenino|Otro)$", ErrorMessage = "El campo {0} debe ser 'Masculino', 'Femenino' u 'Otro'.")]
        [Column("Genero")]
        public string Genero { get; set; } = string.Empty;

        [Range(0.01, 100000.00, ErrorMessage = "El campo {0} debe ser un número positivo válido.")]
        [Column("Salario", TypeName = "decimal(10, 2)")]
        public decimal Salario { get; set; }

        [Column("Foto")]
        public string? Foto { get; set; }

        [Column("FK_IdEstado")]
        public int Estado { get; set; }

        public virtual ICollection<UsuarioViewModel>? Usuarios { get; set; }
    }
}
