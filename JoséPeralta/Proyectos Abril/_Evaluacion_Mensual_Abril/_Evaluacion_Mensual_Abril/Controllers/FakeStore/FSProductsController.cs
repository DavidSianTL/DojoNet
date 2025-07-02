using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Services.FakeStore;
using _Evaluacion_Mensual_Abril.Models.FakeStore;
using _Evaluacion_Mensual_Abril.Models;

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

        public async Task<IActionResult> Update(int id)
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


                    var product = await _fsProductsService.GetProductByIdAsync(id);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    return View("~/Views/FakeStore/Update.cshtml", product);
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
            try
            {
                var createdProduct = await _fsProductsService.AddProductAsync(product);
                if (createdProduct != null)
                {
                    RegistrarLog("Crear Producto", $"Producto creado: {createdProduct.Title} (ID: {createdProduct.Id})");
                    return RedirectToAction("Index");
                }

                RegistrarLog("Crear Producto", "Error al crear producto: datos inválidos o respuesta nula.");
                return View(product);
            }
            catch (Exception e)
            {
                RegistrarLog("Crear Producto", $"Error inesperado: {e.Message}");
                return View(product);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, FSProductsViewModel product)
        {
            try
            {
                var updatedProduct = await _fsProductsService.UpdateProductAsync(id, product);
                if (updatedProduct != null)
                {
                    RegistrarLog("Actualizar Producto", $"Producto actualizado: {updatedProduct.Title} (ID: {updatedProduct.Id})");
                    return RedirectToAction("Index");
                }

                RegistrarLog("Actualizar Producto", $"Error al actualizar producto con ID: {id}");
                return View(product);
            }
            catch (Exception e)
            {
                RegistrarLog("Actualizar Producto", $"Error inesperado: {e.Message}");
                return View(product);
            }
        }


        [HttpGet] // CAMBIA el atributo para no usar GET
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var success = await _fsProductsService.DeleteProductAsync(id);
                if (success)
                {
                    RegistrarLog("Eliminar Producto", $"Producto eliminado con ID: {id}");
                    return RedirectToAction("Index");
                }

                RegistrarLog("Eliminar Producto", $"Error: no se pudo eliminar el producto con ID: {id}");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                RegistrarLog("Eliminar Producto", $"Error inesperado al eliminar producto con ID: {id}. Detalle: {e.Message}");
                return RedirectToAction("Index");
            }
        }






    }
}
