using CRUD_Usuarios_simple.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Usuarios_simple.Controllers
{
    public class CrearController : Controller
    {
        // Cambiamos el modificador a public para poder acceder desde otros controladores
        public static List<UsuarioViewModel> Usuarios = new List<UsuarioViewModel>();

        public IActionResult Crear()
        {
            return View(new UsuarioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearUsuario(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Id = Usuarios.Count + 1;
                Usuarios.Add(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View("Crear", usuario);
        }

        public IActionResult Index()
        {
            return View(Usuarios);
        }
    }
}
