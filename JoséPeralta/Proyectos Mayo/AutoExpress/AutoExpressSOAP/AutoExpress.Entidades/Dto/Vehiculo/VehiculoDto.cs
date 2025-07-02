using System;
using System.ComponentModel.DataAnnotations;

namespace AutoExpressSOAP.Dto
{
    public class VehiculoDto
    {
        public int IdVehiculo { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es requerido")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es requerido")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El tipo de vehículo es requerido")]
        public int IdTipoVehiculo { get; set; }

        public string TipoVehiculo { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public int IdEstado { get; set; }

        public string Estado { get; set; }
    }
}
