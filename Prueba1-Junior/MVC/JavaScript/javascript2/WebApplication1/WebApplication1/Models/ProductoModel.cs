using Microsoft.AspNetCore.Authentication;

namespace pruebaJs2.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
    }
}
