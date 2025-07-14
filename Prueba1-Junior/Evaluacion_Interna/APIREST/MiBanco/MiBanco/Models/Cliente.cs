using System.ComponentModel.DataAnnotations;

namespace MiBanco.Models
{
	public class Cliente
	{
		[Key]
		public int Id { get; set; }
        [Required]
		public string? DPI { get; set; } = null;
		
		[Required]
		public string? Nombre { get; set; } = null;

		[Required]
		public string? Apellido { get; set; } = null;

		[Required]
		public string NIT { get; set; }

		[Required]
		public string? Direccion { get; set; } = null;

		public int Estado { get; set; }
	}
}
