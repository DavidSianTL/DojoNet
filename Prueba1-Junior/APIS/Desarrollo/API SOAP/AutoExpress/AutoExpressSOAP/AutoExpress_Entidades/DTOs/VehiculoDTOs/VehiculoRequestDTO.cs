using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoExpress_Entidades.DTOs.VehiculoDTOs
{
    public class VehiculoRequestDTO
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int anio { get; set; }
        public decimal Precio { get; set; }
    }
}