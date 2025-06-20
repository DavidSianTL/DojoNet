using System.ComponentModel.DataAnnotations;

namespace ClinicaApi.Models
{
	public class Paciente
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Email { get; set; }
		public DateTime FechaNacimiento { get; set; }
		[Required(ErrorMessage = "El teléfono es obligatorio")]
		[Phone(ErrorMessage = "Formato de teléfono inválido")]
		public string Telefono { get; set; }

	}
}
