using System.ComponentModel.DataAnnotations;

namespace MiBanco.Data.DTOs.ClienteDTOs
{
	public class ClienteRequestDTO
	{

		[Required(ErrorMessage = "El campo {0} no puede ser nulo o un espacio en blanco.")]
		[RegularExpression(@"^[1234567890]+$", ErrorMessage = "El campo {0} sólo puede contener nmeros.")]
		[MinLength(13, ErrorMessage = "El campo {0} sólo puede contener 13 dígitos.")]
		[MaxLength(13, ErrorMessage = "El campo {0} sólo puede contener 13 dígitos.")]
		public string DPI { get; set; } = string.Empty;

		[Required(ErrorMessage ="El campo {0} no puede ser nulo o un espacio en blanco.")]
		[StringLength(50, ErrorMessage = "El campo {0} no puede exceder los 50 caracteres.")]
		[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "El campo {0} sólo puede contener letras.")]
		public string? Nombre { get; set; } 

		[Required(ErrorMessage ="El campo {0} no puede ser nulo o un espacio en blanco.")]
		[StringLength(50, ErrorMessage = "El campo {0} no puede exceder los 50 caracteres.")]
		[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$", ErrorMessage = "El camppo {0} sólo puede contener letras.")]
		public string? Apellido { get; set; } = string.Empty;

		[Required(ErrorMessage = "El campo {0} no puede ser nulo o un espacio en blanco.")]
		[RegularExpression(@"^[1234567890]+$", ErrorMessage = "El campo {0} sólo puede contener nmeros.")]
		[MinLength(9, ErrorMessage = "El campo {0} sólo puede contener 9 dígitos.")]
		[MaxLength(9, ErrorMessage = "El campo {0} sólo puede contener 9 dígitos.")]
		public string NIT { get; set; } = string.Empty;
		
		[Required]
		[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúñ#1234567890, ]+$", ErrorMessage = "El camppo {0} sólo puede contener letras.")]
		[StringLength(120, ErrorMessage = "El campo {0} no puede exceder los 120 caracteres.")]
		public string? Direccion { get; set; } = string.Empty;
	}
}
