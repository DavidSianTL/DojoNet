using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiendaEnLinea_MR.Models;

namespace TiendaEnLinea_MR.Services
{
    public class ProductoServicioModel
    {
        private readonly string _productosFile = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

        //Función para ver el listado de productos en el apartado de Ver
        public List<ProductoViewModel> ObtenerProductos()
        {
            if (!File.Exists(_productosFile))
            {
                return new List<ProductoViewModel>();
            }

            var jsonData = File.ReadAllText(_productosFile);
            return JsonConvert.DeserializeObject<List<ProductoViewModel>>(jsonData) ?? new List<ProductoViewModel>();
        }

        //Función para guardar un producto en el archivo Json
        public void GuardarProducto(ProductoViewModel producto)
        {
            var productos = ObtenerProductos();
            producto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
            productos.Add(producto);
            File.WriteAllText(_productosFile, JsonConvert.SerializeObject(productos, Formatting.Indented));
        }

        //Función para buscar un producto por medio del id para el apartado de editar
        public ProductoViewModel? ObtenerProductoPorId(int id)
        {
            var productos = ObtenerProductos();
            return productos.FirstOrDefault(p => p.Id == id);
        }

        //Función para actualizar un producto despues de editarlo
        public void ActualizarProducto(ProductoViewModel productoActualizado)
        {
            var productos = ObtenerProductos();
            var productoExistente = productos.FirstOrDefault(p => p.Id == productoActualizado.Id);

            if (productoExistente != null)
            {
                productoExistente.NombreProducto = productoActualizado.NombreProducto;
                productoExistente.Descripcion = productoActualizado.Descripcion;
                productoExistente.Categoria = productoActualizado.Categoria;
                productoExistente.Cantidad = productoActualizado.Cantidad;
                File.WriteAllText(_productosFile, JsonConvert.SerializeObject(productos, Formatting.Indented));
            }
        }

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



