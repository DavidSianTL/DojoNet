using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoExpressSOAP.Models
{
    [Table("Vehiculos")]
    internal class VehiculosViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public decimal Precio { get; set; }
        public int IdTipoVehiculo { get; set; }
        public int IdEstado { get; set; }
        public TipoVehiculosViewModel TipoVehiculo { get; set; }
        public EstadosViewModel Estado { get; set; }
    }
}