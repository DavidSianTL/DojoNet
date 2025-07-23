using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.Empleados
{
    public class EmpleadosEmpresaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdEmpleadoEmpresa")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int IdEmpleadoEmpresa { get; set; }

        // --- Para la BD y el DAO ---
        [Column("FK_IdEmpresa")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int FK_IdEmpresa { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column("FK_IdEmpleado")]
        public int FK_IdEmpleado { get; set; }


        // Propiedades de navegación hacia Empleado y Empresa
        /*[ForeignKey("FK_IdEmpleado")]
        public EmpleadoViewModel? Empleado { get; set; }

        [ForeignKey("FK_IdDepartamento")]
        public EmpresaViewModel? Empresa { get; set; }*/

    }
}
