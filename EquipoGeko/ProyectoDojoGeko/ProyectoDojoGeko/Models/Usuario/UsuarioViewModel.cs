using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.Usuario
{
    [Table("Usuarios")]
    public class UsuarioViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Column("Username")]
        public string Username { get; set; } = string.Empty;

        // [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Column("Contrasenia")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        // El true es igual a 1 y el false es igual a 0
        [Column("Estado")]
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("FK_IdEmpleado")]
        public int FK_IdEmpleado { get; set; }

        // Propiedad de navegación
        [ForeignKey("FK_IdEmpleado")]
        // Esto permite que Entity Framework relacione el usuario con un empleado
        // y se pueda acceder a los datos del empleado desde el usuario.
        public virtual EmpleadoViewModel? Empleado { get; set; }

        // Propiedad de navegación inversa para el token de usuario
        public virtual TokenUsuarioViewModel? TokenUsuario { get; set; }


    }
}
