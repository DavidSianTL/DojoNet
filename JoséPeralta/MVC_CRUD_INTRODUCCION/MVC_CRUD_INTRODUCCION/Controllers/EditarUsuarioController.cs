using Microsoft.AspNetCore.Mvc;
using MVC_CRUD_INTRODUCCION.Models;

namespace MVC_CRUD_INTRODUCCION.Controllers
{
    public class EditarUsuarioController : Controller
    {
        // Lista simulando la base de datos
        private static List<UsuarioViewModel> _usuarios = new List<UsuarioViewModel>();

        public IActionResult EditarUsuarioView(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                
                return NotFound(); // Si no encuentra el usuario, retorna un error 404
            }
            return View("~/Views/Users/EditarUsuarioView.cshtml", usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = _usuarios.FirstOrDefault(x => x.Id == usuario.Id);
                if (usuarioExistente != null)
                {
                    // Actualizar los valores
                    usuarioExistente.Nombre = usuario.Nombre;
                    usuarioExistente.Apellido = usuario.Apellido;
                    usuarioExistente.Correo = usuario.Correo;
                    usuarioExistente.Puesto = usuario.Puesto;
                    usuarioExistente.Direccion = usuario.Direccion;
                    usuarioExistente.FechaNacimiento = usuario.FechaNacimiento;
                    usuarioExistente.Telefono = usuario.Telefono;
                    usuarioExistente.Estado = usuario.Estado;
                }
                return RedirectToAction("Index", "Home"); // Redirigir al listado
            }
            return View("EditarUsuarioView", usuario);
        }
    }
}
