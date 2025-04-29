using Newtonsoft.Json;

namespace ExamDaniel.Models
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

        // Agregar un nuevo producto
        public void Agregar(Producto producto)
        {
            var productos = ObtenerTodos();

            // Asigna un nuevo ID incremental
            producto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;

            productos.Add(producto);
            GuardarTodos(productos);
        }

        // Editar producto
        public void Editar(Producto producto)
        {
            var productos = ObtenerTodos();
            var productoExistente = productos.FirstOrDefault(p => p.Id == producto.Id);

            if (productoExistente != null)
            {
                productoExistente.Nombre = producto.Nombre;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Stock = producto.Stock;
                productoExistente.Precio = producto.Precio;

                GuardarTodos(productos);
            }
        }

        // Eliminar un producto por ID
        public void Eliminar(int id)
        {
            var productos = ObtenerTodos();
            var productoAEliminar = productos.FirstOrDefault(p => p.Id == id);

            if (productoAEliminar != null)
            {
                productos.Remove(productoAEliminar);
                GuardarTodos(productos);
            }
        }
    }
}
