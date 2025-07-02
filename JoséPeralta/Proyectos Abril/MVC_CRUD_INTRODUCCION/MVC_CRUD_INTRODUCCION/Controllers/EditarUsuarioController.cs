using Microsoft.AspNetCore.Mvc;
using MVC_CRUD_INTRODUCCION.Models;

namespace MVC_CRUD_INTRODUCCION.Controllers
{
    public class EditarUsuarioController : Controller
    {
        // Lista simulando la base de datos
        private static List<UsuarioViewModel> _usuarios;

        static EditarUsuarioController()
        {
            // Inicializar la lista con algunos datos de prueba
            _usuarios = new List<UsuarioViewModel>
                {
                    new UsuarioViewModel { Id = 1, Nombre = "Juan", Apellido = "Pérez", FechaNacimiento = new DateTime(1990, 1, 1), Correo = "juan.perez@example.com", Telefono = "12345678", Puesto = "Desarrollador", Direccion = "Calle Falsa 123", Estado = "Activo" },
                    new UsuarioViewModel { Id = 2, Nombre = "María", Apellido = "Gómez", FechaNacimiento = new DateTime(1985, 5, 15), Correo = "maria.gomez@example.com", Telefono = "87654321", Puesto = "Analista", Direccion = "Avenida Siempre Viva 456", Estado = "Activo" }
                };
        }

        [HttpGet("EditarUsuario/EditarUsuarioView/{id}")]
        public IActionResult EditarUsuarioView(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            Console.WriteLine($"Usuario encontrado: {usuario?.Nombre}"); // Para depuración
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
