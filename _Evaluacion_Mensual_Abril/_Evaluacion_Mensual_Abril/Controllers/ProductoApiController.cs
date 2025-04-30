using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Services;
using _Evaluacion_Mensual_Abril.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class ProductoApiController : Controller
    {
        private readonly ProductoApiService _productoApiService;
        private readonly LoggerServices _loggerServices;

        public ProductoApiController(ProductoApiService productoApiService)
        {
            _productoApiService = productoApiService;
            _loggerServices = new LoggerServices(); 
        }
       
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            const int pageSize = 12;
            var productos = await _productoApiService.ObtenerProductosAsync();

            string fechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string usuario = HttpContext.Session.GetString("NombreCompleto") ?? "Anónimo";

            if (!string.IsNullOrEmpty(search))
            {
                productos = productos.Where(p => p.title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                _loggerServices.RegistrarAccion(usuario, $"Búsqueda de productos con el término '{search}' - {fechaHora}");
            }
            else
            {
                _loggerServices.RegistrarAccion(usuario, $"Acceso al listado de productos (sin búsqueda) - Página {page} - {fechaHora}");
            }

            var pagedProductos = productos.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)productos.Count / pageSize);

            return View(pagedProductos);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var producto = await _productoApiService.ObtenerProductoPorId(id);
            string fechaHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string usuario = HttpContext.Session.GetString("NombreCompleto") ?? "Anónimo";

            if (producto == null)
            {
                _loggerServices.RegistrarAccion(usuario, $"Intento fallido de ver detalles de producto ID {id} - {fechaHora}");
                return NotFound();
            }

            _loggerServices.RegistrarAccion(usuario, $"Visualización de detalles del producto '{producto.title}' (ID: {id}) - {fechaHora}");
            return View(producto);
        }
    }
}
