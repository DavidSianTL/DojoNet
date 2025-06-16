using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models
{
	public class EmpleadosEmpresaViewModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("IdEmpleadoEmpresa")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		public int IdEmpleadoEmpresa { get; set; }

		[Column("FK_IdEmpleado")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		public int FK_IdEmpleado { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[Column("FK_IdEmpresa")]
		public int FK_IdEmpresa { get; set; }

        // Propiedades de navegación hacia Empleado y Empresa
        [ForeignKey("FK_IdEmpleado")]
		public EmpleadoViewModel? Empleado { get; set; }
		
		[ForeignKey("FK_IdEmpresa")]
		public EmpresaViewModel? Empresa { get; set; }
	}
}
