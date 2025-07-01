using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExpress.Entidades
{
    public class Modelo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int MarcaId { get; set; }

        // Relación opcional para facilitar acceso a la marca
        public Marca Marca { get; set; }
    }
}