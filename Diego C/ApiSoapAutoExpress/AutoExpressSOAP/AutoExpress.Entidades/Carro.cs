using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress.Entidades
{
    public class Carro
    {
        public int Id { get; set; }
        public int ModeloId { get; set; }
        public int Año { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }

        // Relación opcional para facilitar acceso a detalles del modelo y marca
        public Modelo Modelo { get; set; }
    }
}
