using Evaluacion_Mensual_Abril.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using _Evaluacion_Mensual_Abril.Services;

namespace Evaluacion_Mensual_Abril.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoService _productoService;
        private readonly LoggerServices _loggerServices;

        public ProductosController()
        {
            _productoService = new ProductoService();
            _loggerServices = new LoggerServices();
        }

        private bool UsuarioAutenticado()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("NombreCompleto"));
        }

        private string ObtenerUsuario()
        {
            return HttpContext.Session.GetString("NombreCompleto") ?? "Anónimo";
        }

        private string FechaHoraActual()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

       
        public IActionResult Index()
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Accedió al listado de productos - {FechaHoraActual()}");

            var productos = _productoService.ObtenerProductos();
            return View(productos);
        }

       
        public IActionResult Crear()
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Accedió a la vista de creación de producto - {FechaHoraActual()}");

            return View();
        }

      
        [HttpPost]
        public IActionResult Crear(ProductoViewModel producto)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            if (ModelState.IsValid)
            {
                var productos = _productoService.ObtenerProductos();
                producto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
                productos.Add(producto);
                _productoService.GuardarProductos(productos);

                _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Creó el producto '{producto.Titulo}' (ID: {producto.Id}) - {FechaHoraActual()}");

                return RedirectToAction("Index");
            }

            return View(producto);
        }

        public IActionResult Editar(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            var producto = _productoService.ObtenerProductos().FirstOrDefault(p => p.Id == id);
            if (producto == null) return NotFound();

            _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Accedió a editar el producto '{producto.Titulo}' (ID: {id}) - {FechaHoraActual()}");

            return View(producto);
        }

        
        [HttpPost]
        public IActionResult Editar(ProductoViewModel producto)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            var productos = _productoService.ObtenerProductos();
            var existente = productos.FirstOrDefault(p => p.Id == producto.Id);
            if (existente == null) return NotFound();

            if (ModelState.IsValid)
            {
                existente.Titulo = producto.Titulo;
                existente.Precio = producto.Precio;
                existente.Descripcion = producto.Descripcion;
                existente.Categoria = producto.Categoria;
                existente.Imagen = producto.Imagen;

                _productoService.GuardarProductos(productos);

                _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Editó el producto '{producto.Titulo}' (ID: {producto.Id}) - {FechaHoraActual()}");

                return RedirectToAction("Index");
            }

            return View(producto);
        }

       
        public IActionResult Eliminar(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            var producto = _productoService.ObtenerProductos().FirstOrDefault(p => p.Id == id);
            if (producto == null) return NotFound();

            _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Accedió a eliminar el producto '{producto.Titulo}' (ID: {id}) - {FechaHoraActual()}");

            return View(producto);
        }

       
        [HttpPost, ActionName("Eliminar")]
        public IActionResult EliminarConfirmado(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            var productos = _productoService.ObtenerProductos();
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto != null)
            {
                productos.Remove(producto);
                _productoService.GuardarProductos(productos);

                _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Eliminó el producto '{producto.Titulo}' (ID: {id}) - {FechaHoraActual()}");
            }

            return RedirectToAction("Index");
        }

      
        public IActionResult Detalles(int id)
        {
            if (!UsuarioAutenticado()) return RedirectToAction("Login", "Login");

            var producto = _productoService.ObtenerProductos().FirstOrDefault(p => p.Id == id);
            if (producto == null) return NotFound();

            _loggerServices.RegistrarAccion(ObtenerUsuario(), $"Consultó detalles del producto '{producto.Titulo}' (ID: {id}) - {FechaHoraActual()}");

            return View(producto);
        }
    }
}
