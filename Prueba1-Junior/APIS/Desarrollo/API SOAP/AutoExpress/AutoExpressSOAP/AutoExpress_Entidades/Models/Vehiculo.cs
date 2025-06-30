using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoExpress_Entidades.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int anio { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
    }
}
