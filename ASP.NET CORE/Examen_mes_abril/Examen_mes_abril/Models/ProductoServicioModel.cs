using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Examen_mes_abril.Models;

namespace Examen_mes_abril.Models
{
    public class ProductoServicioModel
    {
        private readonly string _productosFile = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

        //Función para poder listar todos los productos que se encuentran en el archivo json
        public List<ProductoViewModel> ObtenerProductos()
        {
            if (!File.Exists(_productosFile))
            {
                return new List<ProductoViewModel>();
            }
            var jsonData = File.ReadAllText(_productosFile);
            return JsonConvert.DeserializeObject<List<ProductoViewModel>>(jsonData) ?? new List<ProductoViewModel>();
        }

        //Función para guardar un producto nuevo en el archivo json
        public void GuardarProducto(ProductoViewModel producto)
        {
            var productos = ObtenerProductos();
            producto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1; //Guarda el producto agregando un id 
            productos.Add(producto);
            File.WriteAllText(_productosFile, JsonConvert.SerializeObject(productos, Formatting.Indented));
        }

        //Función para obtener un producto por medio del id
        public ProductoViewModel? ObtenerProductoPorId(int id)
        {
            var productos = ObtenerProductos();
            return productos.FirstOrDefault(p => p.Id == id);
        }

        //Función para actualizar un producto despues de ser editado
        public void ActualizarProducto(ProductoViewModel productoActualizado)
        {
            var productos = ObtenerProductos();
            var productoExistente = productos.FirstOrDefault(p => p.Id == productoActualizado.Id);

            if (productoExistente != null)
            {
                productoExistente.NombreProducto = productoActualizado.NombreProducto;
                productoExistente.Descripcion = productoActualizado.Descripcion;
                productoExistente.Precio = productoActualizado.Precio;
                productoExistente.Cantidad = productoActualizado.Cantidad;
                productoExistente.Talla = productoActualizado.Talla;
                productoExistente.Color = productoActualizado.Color;
                File.WriteAllText(_productosFile, JsonConvert.SerializeObject(productos, Formatting.Indented));
            }
        }

        //Función para eliminar un producto por medio del id
        public bool EliminarProducto(int id)
        {
            var productos = ObtenerProductos();
            var producto = productos.FirstOrDefault(p => p.Id == id);

            if (producto != null)
            {
                productos.Remove(producto);
                File.WriteAllText(_productosFile, JsonConvert.SerializeObject(productos, Formatting.Indented));
                return true;
            }
            return false;
        }
    }
}
