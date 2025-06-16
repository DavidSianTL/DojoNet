using Microsoft.AspNetCore.Mvc;

namespace ProyectoDojoGeko.Controllers
{
    public class ConfiguracionController : Controller
    {
        // GET: Configuracion
        public IActionResult Index()
        {
            ViewBag.Title = "Configuración - En Construcción";
            return View();
        }

        // GET: Configuracion/General
        public IActionResult General()
        {
            ViewBag.Title = "Configuración General";
            return View();
        }

        // GET: Configuracion/Usuario
        public IActionResult Usuario()
        {
            ViewBag.Title = "Configuración de Usuario";
            return View();
        }

        // GET: Configuracion/Notificaciones
        public IActionResult Notificaciones()
        {
            ViewBag.Title = "Configuración de Notificaciones";
            return View();
        }

        // GET: Configuracion/Seguridad
        public IActionResult Seguridad()
        {
            ViewBag.Title = "Configuración de Seguridad";
            return View();
        }

        // POST: Configuracion/Guardar
        [HttpPost]
        public IActionResult Guardar(IFormCollection form)
        {
            try
            {
                // Aquí iría la lógica para guardar la configuración
                // Por ejemplo, guardar en base de datos o archivo de configuración
                
                TempData["Mensaje"] = "Configuración guardada exitosamente";
                TempData["TipoMensaje"] = "success";
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al guardar la configuración: " + ex.Message;
                TempData["TipoMensaje"] = "error";
                
                return RedirectToAction("Index");
            }
        }

        // GET: Configuracion/RestaurarDefecto
        public IActionResult RestaurarDefecto()
        {
            try
            {
                // Aquí iría la lógica para restaurar configuración por defecto
                
                TempData["Mensaje"] = "Configuración restaurada a valores por defecto";
                TempData["TipoMensaje"] = "info";
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al restaurar configuración: " + ex.Message;
                TempData["TipoMensaje"] = "error";
                
                return RedirectToAction("Index");
            }
        }
    }
}