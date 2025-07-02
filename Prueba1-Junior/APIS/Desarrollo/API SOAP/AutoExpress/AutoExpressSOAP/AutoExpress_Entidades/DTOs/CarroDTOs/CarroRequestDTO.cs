using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress_Entidades.DTOs
{
    [Serializable]
    public class CarroRequestDTO
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public decimal Precio { get; set; }

        public CarroRequestDTO() { }
    }
}
