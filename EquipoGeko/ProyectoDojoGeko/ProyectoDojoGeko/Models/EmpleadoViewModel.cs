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
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [Column("NombreEmpleado")]
        public string NombreEmpleado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [Column("Correo")]
        public string Correo { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Column("FechaIngreso")]
        public DateTime FechaIngreso { get; set; } = DateTime.Now;


        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        [Column("FechaNacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("Telefono")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(10, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Column("Genero")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("Salario", TypeName = "decimal(10, 2)")]
        public decimal Salario { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; } = true;

        // Propiedad de navegación inversa
        // Esta propiedad permite acceder a los usuarios asociados a este empleado
        public virtual ICollection<UsuarioViewModel>? Usuarios { get; set; }

        // Propiedad de navegación inversa
        // Esta propiedad permite acceder a los departamentos asociados a este empleado
        // public virtual ICollection<EmpleadoDepartamentoViewModel>? EmpleadosDepartamentos { get; set; }
    }

}

