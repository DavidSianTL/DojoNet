using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Practica3.Models
{
    public class ProductoServicio
    {
        private readonly string _archivoProductos = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

        public List<Producto> ObtenerTodos()
        {
            if (!File.Exists(_archivoProductos))
                return new List<Producto>();

            var json = File.ReadAllText(_archivoProductos);
            return JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
        }

        public void GuardarTodos(List<Producto> productos)
        {
            var json = JsonConvert.SerializeObject(productos, Formatting.Indented);
            File.WriteAllText(_archivoProductos, json);
        }

        public void Agregar(Producto nuevoProducto)
        {
            var productos = ObtenerTodos();
            nuevoProducto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
            productos.Add(nuevoProducto);
            GuardarTodos(productos);
        }

        public Producto? ObtenerProductoPorId(int id) 
        {
            return ObtenerTodos().FirstOrDefault(p => p.Id == id);
        }

        public void ActualizarProducto(Producto productoActualizado)
        {
            var productos = ObtenerTodos();
            var index = productos.FindIndex(p => p.Id == productoActualizado.Id);
            if (index != -1)
            {
                productos[index] = productoActualizado;
                GuardarTodos(productos);
            }
        }

        public void Eliminar(int id)
        {
            var productos = ObtenerTodos();
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                productos.Remove(producto);
                GuardarTodos(productos);
            }
        }
    }
}