using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress.Entidades.Modelos
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public decimal Precio { get; set; }
        public int Fk_IdTipoVehiculo { get; set; }
        public int Fk_IdEstado { get; set; }
        public int Fk_IdPais { get; set; }
    }
}

