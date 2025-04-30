using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Models;
using System.IO;
using _Evaluacion_Mensual_Abril;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class ProductsController : Controller
    {

        // Constructor de la clase ProductsController
        // Se inyecta el modelo de usuario para su uso en la clase
        private readonly UserViewModel _usuario;


        public ProductsController(UserViewModel usuario)
        {
            _usuario = usuario;
        }

        // Función global para obtener el nombre completo del usuario
        public string NombreCompletoLog()
        {
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            return nombreCompleto != null ? $"[Usuario: {nombreCompleto}]" : "[Usuario: No identificado]";
        }

        // Función para registrar en log.txt
        private void RegistrarLog(string accion, string descripcion)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {NombreCompletoLog()} [Acción: {accion}] {descripcion}{Environment.NewLine}";
            System.IO.File.AppendAllText("log.txt", logEntry);
        }

        // Acción que maneja la vista de productos
        public IActionResult Products()
        {
            try
            {
                var userNombre = HttpContext.Session.GetString("UsrNombre");
                var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

                if (userNombre != null)
                {
                    ViewBag.usrNombre = userNombre;
                    ViewBag.NombreCompleto = nombreCompleto;
                    ViewData["isLoggedIn"] = true;

                    var productosJSON = new ProductsJSON();
                    var productos = productosJSON.ObtenerProductos();

                    RegistrarLog("Ver Productos", "Se accedió correctamente a la vista de productos.");
                    return View("~/Views/Products/Products.cshtml", productos);
                }
                else
                {
                    RegistrarLog("Acceso Denegado", "Intento de acceder sin sesión activa.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en Products(): {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }

        // Acción que maneja la vista para crear un nuevo producto
        public IActionResult Create()
        {
            try
            {
                if (HttpContext.Session.GetString("UsrNombre") != null)
                {
                    RegistrarLog("Acceso Crear Producto", "El usuario accedió a la vista de creación.");
                    return View();
                }
                else
                {
                    RegistrarLog("Acceso Denegado", "Intento de acceder a vista de creación sin sesión.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en Create(): {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }

        // Acción que maneja la vista para editar un producto existente
        public IActionResult Update(int id)
        {
            try
            {
                if (HttpContext.Session.GetString("UsrNombre") != null)
                {
                    var productoService = new ProductService();
                    var producto = productoService.ObtenerProductoPorId(id);

                    if (producto == null)
                    {
                        RegistrarLog("Producto No Encontrado", $"No se encontró el producto con ID {id}.");
                        return NotFound("Producto no encontrado.");
                    }

                    RegistrarLog("Editar Producto", $"Acceso a vista de edición para el producto ID {id}.");
                    return View(producto);
                }
                else
                {
                    RegistrarLog("Acceso Denegado", "Intento de editar sin sesión.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en Update(): {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }


        // Acción que maneja la vista para agregar un nuevo producto
        // Verifica si el usuario está autenticado a través de la sesión
        // Si el usuario está autenticado, muestra la vista para agregar un producto
        // Si no está autenticado, redirige al usuario a la página de inicio de sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(ProductsViewModel producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var productoService = new ProductService();
                    productoService.AgregarProducto(producto);

                    RegistrarLog("Producto Creado", $"Producto agregado: {producto.Nombre}");
                    return RedirectToAction("Products");
                }
                else
                {
                    RegistrarLog("Error Validación", "No se pudo agregar el producto. Datos inválidos.");
                    return View("Create", producto);
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en CreateProduct(): {e.Message}");
                return RedirectToAction("Products");
            }
        }

        // GET: Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateProduct(ProductsViewModel producto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var productoService = new ProductService();
                    productoService.EditarProducto(producto);

                    RegistrarLog("Producto Editado", $"Producto editado ID: {producto.Codigo}");
                    return RedirectToAction("Products");
                }
                else
                {
                    RegistrarLog("Error Validación", "No se pudo editar el producto. Datos inválidos.");
                    return View("Editar", producto);
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en UpdateProduct(): {e.Message}");
                return RedirectToAction("Products");
            }
        }


        // DELETE: Eliminar un producto
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var productoService = new ProductService();
                var producto = productoService.ObtenerProductoPorId(id);

                if (producto == null)
                {
                    RegistrarLog("Producto No Encontrado", $"No se encontró el producto con ID {id} para eliminar.");
                    return NotFound($"El producto con ID {id} no existe.");
                }

                var eliminado = productoService.EliminarProducto(id);
                if (eliminado)
                {
                    RegistrarLog("Producto Eliminado", $"Producto eliminado ID: {id}");
                    return RedirectToAction("Products");
                }
                else
                {
                    RegistrarLog("Error Eliminación", $"Error al eliminar producto ID: {id}");
                    return StatusCode(500, $"No se pudo eliminar el producto con ID {id}.");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en Delete(): {e.Message}");
                return RedirectToAction("Products");
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var productos = new ProductsJSON().ObtenerProductos();
            return Ok(productos);
        }

        [HttpGet("porCategoria")]
        public IActionResult PorCategoria()
        {
            var productos = new ProductsJSON().ObtenerProductos();
            var agr = productos
              .GroupBy(p => p.Categoria)
              .Select(g => new { categoria = g.Key, cantidad = g.Count() })
              .ToList();
            return Ok(agr);
        }



    }

}
