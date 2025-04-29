using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text.Json;

namespace _Evaluacion_Mensual_Abril.Models
{
    public class ProductsViewModel
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "users.json");

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Stock { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Proveedor { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        public UserViewModel ValidateUser(string usrnombre, string password)
        {
            var users = JsonConvert.DeserializeObject<List<UserViewModel>>(File.ReadAllText(_ArchivoUsuario));
            return users?.FirstOrDefault(u => u.UsrNombre == usrnombre && u.Password == password);
        }

    }

    public class ProductsJSON
    {
        // Se define la ruta al archivo 'productos.json' donde se almacenan los productos.
        // Utiliza el directorio actual y lo combina con el nombre del archivo.
        private readonly string _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

        // Método para obtener la lista de productos desde el archivo JSON
        public List<ProductsViewModel> ObtenerProductos()
        {
            if (File.Exists(_rutaArchivo))
            {
                var json = System.IO.File.ReadAllText(_rutaArchivo);

                var productos = JsonConvert.DeserializeObject<List<ProductsViewModel>>(json);

                return productos ?? new List<ProductsViewModel>();
            }


            // Si el archivo no existe, devuelve una lista vacía de ProductoModel.
            return new List<ProductsViewModel>();
        }

    }

    public class ProductService
    {
        private readonly string _rutaArchivo;

        public ProductService()
        {
            _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "dbproductos.json");
        }

        public List<ProductsViewModel> ObtenerProductos()
        {
            if (!File.Exists(_rutaArchivo))
            {
                return new List<ProductsViewModel>();
            }

            var jsonData = File.ReadAllText(_rutaArchivo);
            return System.Text.Json.JsonSerializer.Deserialize<List<ProductsViewModel>>(jsonData) ?? new List<ProductsViewModel>();
        }

        // Esta función retorna un único producto basado en su ID
        // Por lo tanto, el tipo de retorno es ProductosViewModel
        public ProductsViewModel ObtenerProductoPorId(int id)
        {
            var productos = ObtenerProductos(); // Devuelve una lista de productos

            return productos.FirstOrDefault(p => p.Codigo == id); // Devuelve un único producto
        }

        public void AgregarProducto(ProductsViewModel producto)
        {
            var productos = ObtenerProductos();
            productos.Add(producto);

            var jsonData = System.Text.Json.JsonSerializer.Serialize(productos);
            File.WriteAllText(_rutaArchivo, jsonData);
        }

        public void EditarProducto(ProductsViewModel producto)
        {
            var productos = ObtenerProductos();
            var productoExistente = productos.FirstOrDefault(p => p.Codigo == producto.Codigo);
            if (productoExistente != null)
            {
                productoExistente.Nombre = producto.Nombre;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Precio = producto.Precio;
                productoExistente.Categoria = producto.Categoria;
                productoExistente.Stock = producto.Stock;
                productoExistente.Proveedor = producto.Proveedor;
                productoExistente.FechaIngreso = producto.FechaIngreso;
                var jsonData = System.Text.Json.JsonSerializer.Serialize(productos);
                File.WriteAllText(_rutaArchivo, jsonData);
            }
        }

        public bool EliminarProducto(int id)
        {
            var productos = ObtenerProductos(); // Obtiene la lista de productos desde el archivo JSON
            var productoExistente = productos.FirstOrDefault(p => p.Codigo == id);
            if (productoExistente != null)
            {
                productos.Remove(productoExistente); // Elimina el producto de la lista
                var jsonData = System.Text.Json.JsonSerializer.Serialize(productos);
                File.WriteAllText(_rutaArchivo, jsonData); // Actualiza el archivo JSON
                return true; // Indica que la eliminación fue exitosa
            }
            return false; // Indica que no se encontró el producto
        }


    }


}
