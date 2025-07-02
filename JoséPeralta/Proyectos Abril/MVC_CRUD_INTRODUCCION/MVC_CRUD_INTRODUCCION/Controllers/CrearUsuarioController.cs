using Microsoft.AspNetCore.Mvc;
using MVC_CRUD_INTRODUCCION.Models;

namespace MVC_CRUD_INTRODUCCION.Controllers
{
    public class CrearUsuarioController : Controller
    {
        // Usar la misma lista estática para compartir los datos entre controladores
        private static List<UsuarioViewModel> _usuarios = new List<UsuarioViewModel>();

        public IActionResult CrearUsuarioView()
        {
            // Se envía un nuevo objeto UsuarioViewModel a la vista
            // return View(new UsuarioViewModel());
            return View("~/Views/Users/CrearUsuarioView.cshtml", new UsuarioViewModel());
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
            // return View("CrearUsuarioView", usuario);
            return View("CrearUsuarioView", usuario);
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml", _usuarios);
        }

    }
}
