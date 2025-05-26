using System.ComponentModel.DataAnnotations;

namespace DatabaseConnection.Models
{
	public class Empleado
	{
		public int EmpleadoID { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string Apellido {  get; set; } = string.Empty;
		public DateTime FechaNacimiento { get; set; } = DateTime.Now;
		public DateTime FechaIngreso { get; set; } = DateTime.Now;
		public string Puesto { get; set; } = string.Empty;
		public decimal SalarioBase { get; set; } = decimal.Zero;
		public int Activo { get; set; } = 1;
		
	}
}
