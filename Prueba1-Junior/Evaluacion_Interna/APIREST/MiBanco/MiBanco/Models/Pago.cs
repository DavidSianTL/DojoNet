using System.ComponentModel.DataAnnotations;

namespace MiBanco.Models
{
	public class Pago
	{
		[Key]
		[Required]
		public int Id { get; set; }

		[Required]
		public string FK_IdCuentaOrigen { get; set; }

		[Required]
		public string FK_IdServicio { get; set; }

		[Required]
		public decimal Monto { get; set; }
		[Required]
        public string Descripcion { get; set; }

        [Required]
		public DateTime Fecha { get; set; }

        [Required]
		public int FK_IdEstado { get; set; }

    }
}
