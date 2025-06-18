using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoDojoGeko.Models.Empleados
{
	public class EmpleadosDepartamentoViewModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("IdEmpleadosDepartamento")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		public int IdEmpleadosDepartamento { get; set; }

    	// --- Para la BD y el DAO ---
		[Column("FK_IdEmpleado")]
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		public int FK_IdEmpleado { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[Column("FK_IdDepartamento")]
		public int FK_IdDepartamento { get; set; }

    	// --- Para el formulario de selección múltiple ---
		[NotMapped]
		public List<int> FK_IdsEmpleado { get; set; } = new();

		[NotMapped]
		public List<int> FK_IdsDepartamento { get; set; } = new();

        // Propiedades de navegación hacia Empleado y Empresa
        [ForeignKey("FK_IdEmpleado")]
		public EmpleadoViewModel? Empleado { get; set; }
		
		[ForeignKey("FK_IdDepartamento")]
		public EmpresaViewModel? Departamento { get; set; }
	}
}
