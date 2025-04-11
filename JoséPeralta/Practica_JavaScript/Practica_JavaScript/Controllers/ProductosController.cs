using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practica_JavaScript.Models;
using System.IO;
using Practica_JavaScript.Services;

namespace Practica_JavaScript.Controllers
{
    public class ProductosController : Controller
    {

        // Constructor de la clase ProductosController
        // Se inyecta el modelo de usuario para su uso en la clase
        private readonly UsuarioViewModel _usuario;


        public ProductosController(UsuarioViewModel usuario)
        {
            _usuario = usuario;
        }

        // Función global para obtener el nombre completo del usuario
        public string NombreCompletoLog()
        {
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            if (nombreCompleto != null)
            {
                return "Usuario: " + nombreCompleto;
            }
            else
            {
                return "No se pudo acceder al nombre completo del usuario.";
            }
        }

        // Acción que maneja la vista de productos
        // Verifica si el usuario está autenticado a través de la sesión
        // Si el usuario está autenticado, obtiene la lista de productos
        // y los pasa a la vista
        // Si no está autenticado, redirige al usuario a la página de inicio de sesión
        public IActionResult Productos()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                ViewData["isLoggedIn"] = isLoggedIn;

                // Instancia el servicio y obtiene los productos
                var productosJSON = new ProductosJSON();
                var productos = productosJSON.ObtenerProductos();

                // Pasa los productos como modelo a la vista
                return View(productos);
            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder a la vista de Productos" + Environment.NewLine);
                return RedirectToAction("Login", "Login");
            }

        }

        public IActionResult Agregar()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                ViewData["isLoggedIn"] = isLoggedIn;

                // Pasa los productos como modelo a la vista
                return View();
            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder a la vista de Agregar Productos" + Environment.NewLine);
                return RedirectToAction("Login", "Login");
            }
        }

        
        public IActionResult Editar(int id)
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                ViewData["isLoggedIn"] = isLoggedIn;

                // Obtener el producto por ID
                var productoService = new ProductoService();
                var producto = productoService.ObtenerProductoPorId(id);

                if (producto == null)
                {
                    System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo obtener el producto a editar" + Environment.NewLine);
                    return NotFound("Producto no encontrado.");
                }

                //return View(); // Pasar el producto a la vista

                return View(producto);


            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder a la vista de Editar Productos" + Environment.NewLine);
                return RedirectToAction("Login", "Login");
            }
        }


        // Acción que maneja la vista para agregar un nuevo producto
        // Verifica si el usuario está autenticado a través de la sesión
        // Si el usuario está autenticado, muestra la vista para agregar un producto
        // Si no está autenticado, redirige al usuario a la página de inicio de sesión

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProductos(ProductosViewModel producto)
        {
            
            if (ModelState.IsValid)
            {
                // Guardar el producto en el archivo JSON
                var productoService = new ProductoService();
                productoService.AgregarProducto(producto);
                return RedirectToAction("Productos");
            }
            else
            {
                // Si hay errores de validación, vuelve a mostrar la vista con los errores
                var errorMessage = "Error: No se pudo agregar el producto. Verifique los datos ingresados.";

                System.IO.File.AppendAllText("log.txt", DateTime.Now + NombreCompletoLog() + errorMessage + Environment.NewLine);
                return View("Productos", producto);
            }
        }

        // GET: Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarProducto(ProductosViewModel producto)
        {
            if (ModelState.IsValid)
            {
                var productoService = new ProductoService();

                // Cambiar la asignación implícita a una llamada directa al método void
                productoService.EditarProducto(producto);

                // Redirigir a la acción "Productos" después de la edición
                return RedirectToAction("Productos");

            } 
            else
            {
                // Si hay errores de validación, vuelve a mostrar la vista con los errores
                var errorMessage = "Error: No se pudo editar el producto. Verifique los datos ingresados.";
                System.IO.File.AppendAllText("log.txt", DateTime.Now + NombreCompletoLog() + errorMessage + Environment.NewLine);
                return View("Editar", producto);
            }

            // Si hay errores, vuelve a mostrar la vista con los datos
            //return View(producto);
        }


        // DELETE: Eliminar un producto
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var productoService = new ProductoService();

            var producto = productoService.ObtenerProductoPorId(id);
            if (producto == null)
            {
                return NotFound($"El producto con ID {id} no existe.");
            }

            var eliminado = productoService.EliminarProducto(id);
            if (eliminado)
            {
                // Redirigís a la vista principal
                return RedirectToAction("Productos");
            }
            else
            {
                return StatusCode(500, $"Error: No se pudo eliminar el producto con ID {id}.");
            }

        }



    }

}
