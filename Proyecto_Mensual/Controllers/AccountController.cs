using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppFuturista.Models; // Se actualiza el espacio de nombres
using RazorCrudApp.Services;

namespace RazorCrudApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si el usuario ya está logueado, redirige al dashboard
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Autenticar al usuario
            var user = await _userService.AuthenticateAsync(model.NombreUsuario, model.Contraseña);

            if (user == null)
            {
                ModelState.AddModelError("", "Nombre de usuario o contraseña inválidos");
                return View(model);
            }

            // Almacenar el ID del usuario en la sesión
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
