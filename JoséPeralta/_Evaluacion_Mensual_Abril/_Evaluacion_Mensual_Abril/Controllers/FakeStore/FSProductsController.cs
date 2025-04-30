using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Services.FakeStore;
using _Evaluacion_Mensual_Abril.Models.FakeStore;

namespace _Evaluacion_Mensual_Abril.Controllers.FakeStore
{
    public class FSProductsController : Controller
    {
        private readonly FSProductService _fsProductsService;

        public FSProductsController(FSProductService fsProductService)
        {
            _fsProductsService = fsProductService;
        }

        // Función global para obtener el nombre completo del usuario
        private string NombreCompletoLog()
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

        public async Task<IActionResult> Index()
        {
            try
            {
                var userNombre = HttpContext.Session.GetString("UsrNombre");
                var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

                if (userNombre != null)
                {
                    ViewBag.usrNombre = userNombre;
                    ViewBag.NombreCompleto = nombreCompleto;

                    var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                    ViewData["isLoggedIn"] = isLoggedIn;

                    var products = await _fsProductsService.GetAllProductsAsync();

                    RegistrarLog("Acceso Productos", "Acceso correcto a la vista de productos.");
                    return View("~/Views/FakeStore/Index.cshtml", products);
                }
                else
                {
                    RegistrarLog("Acceso Productos", "Error: Usuario no autenticado al intentar acceder a la vista de productos.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Acceso Productos", $"Error inesperado al cargar productos. Detalle: {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var userNombre = HttpContext.Session.GetString("UsrNombre");
                var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
                if (userNombre != null)
                {
                    ViewBag.usrNombre = userNombre;
                    ViewBag.NombreCompleto = nombreCompleto;
                    var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                    ViewData["isLoggedIn"] = isLoggedIn;
                    return View("~/Views/FakeStore/Create.cshtml");
                }
                else
                {
                    RegistrarLog("Acceso Crear Producto", "Error: Usuario no autenticado al intentar acceder a la vista de crear producto.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Acceso Crear Producto", $"Error inesperado al cargar la vista de crear producto. Detalle: {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }

        public async Task<IActionResult> Update()
        {
            try
            {
                var userNombre = HttpContext.Session.GetString("UsrNombre");
                var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
                if (userNombre != null)
                {
                    ViewBag.usrNombre = userNombre;
                    ViewBag.NombreCompleto = nombreCompleto;
                    var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                    ViewData["isLoggedIn"] = isLoggedIn;
                    return View("~/Views/FakeStore/Create.cshtml");
                }
                else
                {
                    RegistrarLog("Acceso Crear Producto", "Error: Usuario no autenticado al intentar acceder a la vista de crear producto.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Acceso Crear Producto", $"Error inesperado al cargar la vista de crear producto. Detalle: {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(FSProductsViewModel product)
        {
            var createdProduct = await _fsProductsService.AddProductAsync(product);
            if (createdProduct != null)
            {
                return RedirectToAction("Index");
            }
            // Manejar el error
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, FSProductsViewModel product)
        {
            var updatedProduct = await _fsProductsService.UpdateProductAsync(id, product);
            if (updatedProduct != null)
            {
                return RedirectToAction("Index");
            }
            // Manejar el error
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct  (int id)
        {
            var success = await _fsProductsService.DeleteProductAsync(id);
            if (success)
            {
                return RedirectToAction("Index");
            }
            // Manejar el error
            return RedirectToAction("Index");
        }





    }
}
