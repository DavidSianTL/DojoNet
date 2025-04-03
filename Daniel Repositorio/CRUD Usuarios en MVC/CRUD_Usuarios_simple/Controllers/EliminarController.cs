using CRUD_Usuarios_simple.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CRUD_Usuarios_simple.Controllers
{
    public class EliminarController : Controller
    {
        public IActionResult Index()
        {
            // Utilizamos la lista compartida desde CrearController
            return View(CrearController.Usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = CrearController.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                CrearController.Usuarios.Remove(usuario);
            }
            return RedirectToAction("Index", "Crear");
        }
    }
}
