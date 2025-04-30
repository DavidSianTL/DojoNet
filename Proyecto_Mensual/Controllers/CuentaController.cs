using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppFuturista.Models;
using AppFuturista.Services;

namespace AppFuturista.Controllers
{
    public class CuentaController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly BitacoraService _bitacoraService;
        
        public CuentaController(UsuarioService usuarioService, BitacoraService bitacoraService)
        {
            _usuarioService = usuarioService;
            _bitacoraService = bitacoraService;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            // Si el usuario ya está autenticado, redirigir al panel
            if (HttpContext.Session.GetString("UsuarioId") != null)
            {
                return RedirectToAction("Index", "Panel");
            }
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                
                var usuario = await _usuarioService.AutenticarAsync(model.NombreUsuario, model.Contraseña);
                
                if (usuario == null)
                {
                    ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos");
                    return View(model);
                }
                
                // Guardar información del usuario en la sesión
                HttpContext.Session.SetString("UsuarioId", usuario.Id);
                HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
                HttpContext.Session.SetString("NombreCompleto", usuario.NombreCompleto ?? usuario.NombreUsuario);
                HttpContext.Session.SetString("Rol", usuario.Rol);
                
                return RedirectToAction("Index", "Panel");
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("Cuenta", "Error en el proceso de login", ex);
                ModelState.AddModelError("", "Ha ocurrido un error al procesar la solicitud");
                return View(model);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
                
                if (!string.IsNullOrEmpty(nombreUsuario))
                {
                    await _bitacoraService.RegistrarEventoAsync("Autenticación", $"Cierre de sesión: {nombreUsuario}");
                }
                
                // Limpiar la sesión
                HttpContext.Session.Clear();
                
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("Cuenta", "Error en el proceso de logout", ex);
                return RedirectToAction("Login");
            }
        }
    }
}
