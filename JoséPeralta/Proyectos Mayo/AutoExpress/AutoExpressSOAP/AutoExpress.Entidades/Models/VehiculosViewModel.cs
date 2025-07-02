using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoExpress.Entidades.Models
{
    [Table("Vehiculos")]
    public class VehiculosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVehiculo { get; set; }

        [Required(ErrorMessage = "La marca del vehículo es obligatoria.")]
        [StringLength(100, ErrorMessage = "La marca no puede tener más de 100 caracteres.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo del vehículo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El modelo no puede tener más de 100 caracteres.")]
        public string Modelo { get; set; }

        [Range(1900, 2026, ErrorMessage = "El año debe estar entre 1900 y 2026.")]
        public int Anio { get; set; }

        [Range(0, 9999999.99, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El modelo del vehículo es obligatorio.")]
        [StringLength(25, ErrorMessage = "El origen no puede tener más de 25 caracteres.")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de vehículo válido.")]
        public int IdTipoVehiculo { get; set; }

        [Required(ErrorMessage = "El estado del vehículo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado válido.")]
        public int IdEstado { get; set; }

        // Relaciones
        public TipoVehiculosViewModel TipoVehiculo { get; set; }
        public EstadosViewModel Estado { get; set; }
    }
}
