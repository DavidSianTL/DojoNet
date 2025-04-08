using EmpresaLogin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmpresaLogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "usuarios.json");

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || usuario.Edad <= 0)
            {
                TempData["Mensaje"] = "error";
                return RedirectToAction("Index");
            }

            List<Usuario> usuarios = new();
            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json) ?? new List<Usuario>();
            }

            usuarios.Add(usuario);
            System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(usuarios, Formatting.Indented));

            TempData["Mensaje"] = "ok";
            return RedirectToAction("Index");
        }
    }
}
