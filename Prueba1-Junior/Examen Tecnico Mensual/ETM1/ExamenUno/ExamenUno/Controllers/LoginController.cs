using System.Text.Json;
using ExamenUno.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Usuario user)
        {
            var rutaUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Usuarios.json");
            var content = System.IO.File.ReadAllText(rutaUsuarios);
            var users = JsonSerializer.Deserialize<List<Usuario>>(content);

            var validUser = users.FirstOrDefault(u=>
                u.username == user.username &&
                u.password == user.password
            );

            if(validUser != null)
            {
                HttpContext.Session.SetString("User", validUser.username);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
