using System.ComponentModel.DataAnnotations;

namespace MiBanco.Data.DTOs.PagoDTOs
{
	public class PagoRequestDTO
	{
		[Required]
        [RegularExpression(@"^[1234567890]+$", ErrorMessage = "El campo {0} sólo puede contener nmeros.")]
        public string? FK_IdCuentaOrigen { get; set; } = null;

        [Required]
        [RegularExpression(@"^[1234567890]+$", ErrorMessage = "El campo {0} sólo puede contener nmeros.")]
        public string? FK_IdServicio { get; set; } = null;

		[Required]
        [RegularExpression(@"^[1234567890. ]+$", ErrorMessage = "El campo {0} sólo puede contener nmeros.")]
        public decimal Monto { get; set; }
		
		[Required]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ 1234567890]+$", ErrorMessage = "El campo {0} sólo puede contener letras.")]
        public string? Descripcion { get; set; } = null;

    }

}
