using System.ComponentModel.DataAnnotations;

namespace MiBanco.Data.DTOs.ClienteDTOs
{
    public class ClienteResponseDTO
    {
        int Id { get; set; }

        [Required]
        public string? Nombre { get; set; } = null;

        [Required]
        public string? Apellido { get; set; } = null;

        [Required]
        public string? NIT { get; set; }

        [Required]
        public string? Direccion { get; set; } = null;

        public int Estado { get; set; }
    }
}
