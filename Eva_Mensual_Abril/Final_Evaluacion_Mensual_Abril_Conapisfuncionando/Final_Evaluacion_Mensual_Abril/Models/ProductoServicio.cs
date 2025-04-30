
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Evaluacion_Mensual_Abril.Models
{
    public class ProductoServicio
    {
        private readonly string _archivoProductos = Path.Combine(Directory.GetCurrentDirectory(), "products.json");

        public List<Producto> ObtenerTodos()
        {
            if (!File.Exists(_archivoProductos))
            {
                return new List<Producto>();
            }

            var json = File.ReadAllText(_archivoProductos);
            return JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
        }

        public void GuardarTodos(List<Producto> productos)
        {
            var json = JsonConvert.SerializeObject(productos, Formatting.Indented);
            File.WriteAllText(_archivoProductos, json);
        }

        public void Agregar(Producto producto)
        {
            var productos = ObtenerTodos();
            producto.Id = productos.Count > 0 ? productos.Max(p => p.Id) + 1 : 1;
            productos.Add(producto);
            GuardarTodos(productos);
        }

        public void Actualizar(Producto producto)
        {
            var productos = ObtenerTodos();
            var index = productos.FindIndex(p => p.Id == producto.Id);
            if (index != -1)
            {
                productos[index] = producto;
                GuardarTodos(productos);
            }
        }

        public Producto ObtenerPorId(int id)
        {
            var productos = ObtenerTodos();
            return productos.FirstOrDefault(p => p.Id == id);
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