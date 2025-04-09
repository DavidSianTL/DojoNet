using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Text.Json;

namespace Practica_JavaScript.Models
{
    public class ProductosViewModel
    {
        private readonly string _ArchivoUsuario = Path.Combine(Directory.GetCurrentDirectory(), "sessionLogin.json");

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public string Talla { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public string Color { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public string Marca { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public int Stock { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]

        public string Descripcion { get; set; }

        public UsuarioViewModel ValidateUser(string usrnombre, string password)
        {
            var users = JsonConvert.DeserializeObject<List<UsuarioViewModel>>(File.ReadAllText(_ArchivoUsuario));
            return users?.FirstOrDefault(u => u.UsrNombre == usrnombre && u.Password == password);
        }

    }

    public class ProductosJSON
    {
        // Se define la ruta al archivo 'productos.json' donde se almacenan los productos.
        // Utiliza el directorio actual y lo combina con el nombre del archivo.
        private readonly string _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");

        // Método para obtener la lista de productos desde el archivo JSON
        public List<ProductosViewModel> ObtenerProductos()
        {
            if (File.Exists(_rutaArchivo))
            {
                var json = System.IO.File.ReadAllText(_rutaArchivo);

                var productos = JsonConvert.DeserializeObject<List<ProductosViewModel>>(json);

                return productos ?? new List<ProductosViewModel>();
            }


            // Si el archivo no existe, devuelve una lista vacía de ProductoModel.
            return new List<ProductosViewModel>();
        }
        
    }

    public class ProductoService
    {
        private readonly string _rutaArchivo;

        public ProductoService()
        {
            _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "productos.json");
        }

        public List<ProductosViewModel> ObtenerProductos()
        {
            if (!File.Exists(_rutaArchivo))
            {
                return new List<ProductosViewModel>();
            }

            var jsonData = File.ReadAllText(_rutaArchivo);
            return System.Text.Json.JsonSerializer.Deserialize<List<ProductosViewModel>>(jsonData) ?? new List<ProductosViewModel>();
        }

        // Esta función retorna una lista de productos
        /*public List<ProductosViewModel> ObtenerProductoPorId(int id)
        {

            var productos = ObtenerProductos();
            return productos.Where(p => p.Codigo == id).ToList();

        }*/

        // Esta función retorna un único producto basado en su ID
        // Por lo tanto, el tipo de retorno es ProductosViewModel
        // Es por eso que no se retornaba bien el producto si se usaba la función anterior
        public ProductosViewModel ObtenerProductoPorId(int id)
        {
            var productos = ObtenerProductos(); // Devuelve una lista de productos
            return productos.FirstOrDefault(p => p.Codigo == id); // Devuelve un único producto
        }

        public void AgregarProducto(ProductosViewModel producto)
        {
            var productos = ObtenerProductos();
            productos.Add(producto);

            var jsonData = System.Text.Json.JsonSerializer.Serialize(productos);
            File.WriteAllText(_rutaArchivo, jsonData);
        }

        public void EditarProducto(ProductosViewModel producto)
        {
            var productos = ObtenerProductos();
            var productoExistente = productos.FirstOrDefault(p => p.Codigo == producto.Codigo);
            if (productoExistente != null)
            {
                productoExistente.NombreProducto = producto.NombreProducto;
                productoExistente.Precio = producto.Precio;
                productoExistente.Talla = producto.Talla;
                productoExistente.Color = producto.Color;
                productoExistente.Marca = producto.Marca;
                productoExistente.Stock = producto.Stock;
                productoExistente.Descripcion = producto.Descripcion;
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
