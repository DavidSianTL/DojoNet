using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Practica3.Models
{
    public class ProductoServicio
    {
        // Ruta completa al archivo JSON donde se almacenan los productos
        private readonly string _archivoProductos = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

        // Obtiene todos los productos desde el archivo JSON
        public List<Producto> ObtenerTodos()
        {
            if (!File.Exists(_archivoProductos)) // Si el archivo no existe, se retorna una lista vacía
                return new List<Producto>();

            var json = File.ReadAllText(_archivoProductos); // Lee el contenido del archivo
            return JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>(); // Deserializa y retorna la lista
        }

        // Guarda toda la lista de productos en el archivo JSON
        public void GuardarTodos(List<Producto> productos)
        {
            var json = JsonConvert.SerializeObject(productos, Formatting.Indented); // Convierte la lista a JSON con formato legible
            File.WriteAllText(_archivoProductos, json); // Escribe el JSON en el archivo
        }

        // Agrega un nuevo producto a la lista
        public void Agregar(Producto nuevoProducto)
        {
            var productos = ObtenerTodos(); // Carga todos los productos existentes

            // Genera un nuevo ID autoincremental
            nuevoProducto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;

            productos.Add(nuevoProducto); // Agrega el nuevo producto a la lista
            GuardarTodos(productos); // Guarda la lista actualizada
        }

        // Busca y retorna un producto por su ID
        public Producto? ObtenerProductoPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(p => p.Id == id); // Retorna el primer producto que coincida con el ID
        }

        // Actualiza un producto existente
        public void ActualizarProducto(Producto productoActualizado)
        {
            var productos = ObtenerTodos(); // Carga todos los productos
            var index = productos.FindIndex(p => p.Id == productoActualizado.Id); // Busca la posición del producto a actualizar

            if (index != -1)
            {
                productos[index] = productoActualizado; // Reemplaza el producto en esa posición
                GuardarTodos(productos); // Guarda la lista modificada
            }
        }

        // Elimina un producto por su ID
        public void Eliminar(int id)
        {
            var productos = ObtenerTodos(); // Carga los productos
            var producto = productos.FirstOrDefault(p => p.Id == id); // Busca el producto por ID

            if (producto != null)
            {
                productos.Remove(producto); // Elimina el producto encontrado
                GuardarTodos(productos); // Guarda la lista actualizada
            }
        }
    }
}
