using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CRUD_Evaluacion_Mensual_Abril.Models;

namespace CRUD_Evaluacion_Mensual_Abril.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _rutaUsuarios = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "usuarios.json");

        private List<UsuarioModel> ObtenerUsuarios()
        {
            var json = System.IO.File.ReadAllText(_rutaUsuarios);
            return JsonConvert.DeserializeObject<List<UsuarioModel>>(json);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usrNombre, string password)
        {
            var usuarios = ObtenerUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.usrNombre == usrNombre && u.password == password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("usrNombre", usuario.usrNombre);
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Usuario o contraseña incorrectos";
            return RedirectToAction("Index");
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}