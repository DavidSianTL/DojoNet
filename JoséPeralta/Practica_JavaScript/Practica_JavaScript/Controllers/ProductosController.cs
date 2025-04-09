using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Practica_JavaScript.Models;

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

                // Instancia el servicio y obtiene los productos
                var productosJSON = new ProductosJSON();
                var productos = productosJSON.ObtenerProductos();

                // Pasa los productos como modelo a la vista
                return View(productos);
            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder " + Environment.NewLine);
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

                // Pasa los productos como modelo a la vista
                return View();
            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder " + Environment.NewLine);
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult Editar()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                // Pasa los productos como modelo a la vista
                return View();
            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder " + Environment.NewLine);
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
                return View("Productos", producto);
            }
        }

        // GET: Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarProducto()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                return View();

            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder " + Environment.NewLine);
                return RedirectToAction("Login", "Login");
            }
        }

        // DELETE: Eliminar un producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarProducto()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                return View();
            }
            else
            {
                System.IO.File.AppendAllText("log.txt", DateTime.Now + " - Error: No se pudo acceder " + Environment.NewLine);
                return RedirectToAction("Login", "Login");
            }
        }


    }

}
