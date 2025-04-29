using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wsServicioAuth.Models
{
    public class Product
    {
        public int Id { get; set; }      // ID único del producto
        public string Name { get; set; } // Nombre del producto
        public decimal Price { get; set; } // Precio del producto
    }
}