using CRUD_Usuarios_simple.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Usuarios_simple.Controllers
{
    public class CrearController : Controller
    {
        // Usar la misma lista estática para compartir los datos entre controladores
        private static List<UsuarioViewModel> _usuarios = new List<UsuarioViewModel>();

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
                usuario.Id = _usuarios.Count + 1;
                _usuarios.Add(usuario);

                return RedirectToAction(nameof(Index));
            }
            return View("Crear", usuario);
        }

        public IActionResult Index()
        {
            return View(_usuarios);
        }
    }
}
