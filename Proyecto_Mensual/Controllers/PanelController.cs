using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppFuturista.Services;
using AppFuturista.Filters;

namespace AppFuturista.Controllers
{
    [ValidarSesion]
    public class PanelController : Controller
    {
        private readonly ProductoService _productoService;
        private readonly BitacoraService _bitacoraService;
        
        public PanelController(ProductoService productoService, BitacoraService bitacoraService)
        {
            _productoService = productoService;
            _bitacoraService = bitacoraService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var productos = await _productoService.ObtenerTodosAsync();
                return View(productos);
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("Panel", "Error al cargar el panel principal", ex);
                return RedirectToAction("Error", "Home", new { mensaje = "Error al cargar los productos" });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Bitacora()
        {
            try
            {
                // Verificar si el usuario es administrador
                if (HttpContext.Session.GetString("Rol") != "Administrador")
                {
                    return RedirectToAction("AccesoDenegado", "Home");
                }
                
                var eventos = await _bitacoraService.ObtenerEventosAsync();
                return View(eventos);
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("Panel", "Error al cargar la bitácora", ex);
                return RedirectToAction("Error", "Home", new { mensaje = "Error al cargar la bitácora" });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Errores()
        {
            try
            {
                // Verificar si el usuario es administrador
                if (HttpContext.Session.GetString("Rol") != "Administrador")
                {
                    return RedirectToAction("AccesoDenegado", "Home");
                }
                
                var errores = await _bitacoraService.ObtenerErroresAsync();
                return View(errores);
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("Panel", "Error al cargar el registro de errores", ex);
                return RedirectToAction("Error", "Home", new { mensaje = "Error al cargar el registro de errores" });
            }
        }
    }
}
