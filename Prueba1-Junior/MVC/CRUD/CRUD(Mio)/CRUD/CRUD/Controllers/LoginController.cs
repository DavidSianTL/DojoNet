using System.Text.Json;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{

    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Usuario user)
        {
            if (ModelState.IsValid)
            {
                string usersRoute = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Usuarios.json");
                string content = System.IO.File.ReadAllText(usersRoute);

                List<Usuario> usersList = JsonSerializer.Deserialize<List<Usuario>>(content);

                //buscamos un usuario en la lista que coincida
                var validUser = usersList.FirstOrDefault(u =>
                    u.Username == user.Username &&
                    u.Password == user.Password);

                if (validUser != null)
                {
                    HttpContext.Session.SetString("Usuario", validUser.Username);
                    return RedirectToAction("Index", "Home");
                }

            }
            return View();
        }
    }
}
