using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_CRUD_INTRODUCCION.Models;

namespace MVC_CRUD_INTRODUCCION.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Se crea una lista de usuarios para simular una base de datos
        private static List<UsuarioViewModel> _usuarios = new List<UsuarioViewModel>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_usuarios);
        }

        public IActionResult CrearUsuarioView()
        {
            // Se envía un nuevo objeto UsuarioViewModel a la vista
            return View(new UsuarioViewModel());
        }

        public IActionResult EditarUsuarioView()
        {
            return View(new UsuarioViewModel());
        }

        public IActionResult EliminarUsuarioView()
        {
            return View(new UsuarioViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearUsuario(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Id = _usuarios.Count + 1;
                _usuarios.Add(usuario);

                return RedirectToAction(nameof(Index));
            }
            return View("Index", usuario);
        }

        [HttpPut]
        public IActionResult EditarUsuario(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = _usuarios.FirstOrDefault(x => x.Id == usuario.Id);
                if (usuarioExistente != null)
                {
                    usuarioExistente.Nombre = usuario.Nombre;
                    usuarioExistente.Apellido = usuario.Apellido;
                    usuarioExistente.Correo = usuario.Correo;
                    usuarioExistente.Puesto = usuario.Puesto;
                    usuarioExistente.Direccion = usuario.Direccion;
                    usuarioExistente.FechaNacimiento = usuario.FechaNacimiento;
                    usuarioExistente.Telefono = usuario.Telefono;
                    usuarioExistente.Estado = usuario.Estado;
                }
                return RedirectToAction(nameof(Index));
            }
            return View("Index", usuario);
        }

        [HttpPost]
        public IActionResult EliminarUsuario(UsuarioViewModel usuario)
        {
            // Verifica si el modelo es válido
            //if (ModelState.IsValid)
            //{
                // Busca el usuario existente por Id
                var usuarioExistente = _usuarios.FirstOrDefault(x => x.Id == usuario.Id);
                if (usuarioExistente != null)
                {
                    // Elimina el usuario de la lista
                    _usuarios.Remove(usuarioExistente);
                    return RedirectToAction(nameof(Index));
                }
                // Agrega un error al ModelState si el usuario no se encuentra
                ModelState.AddModelError("", "Usuario no encontrado.");
            //}
            // Retorna a la vista de eliminación si el modelo no es válido o el usuario no se encuentra
            return View("EliminarUsuarioView", usuario);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
