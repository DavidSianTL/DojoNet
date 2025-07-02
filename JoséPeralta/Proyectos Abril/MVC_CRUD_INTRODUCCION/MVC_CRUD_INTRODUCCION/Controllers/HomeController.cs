using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_CRUD_INTRODUCCION.Models;

namespace MVC_CRUD_INTRODUCCION.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Hacer la lista est�tica para compartirla entre controladores
        // private static List<UsuarioViewModel> _usuarios = new List<UsuarioViewModel>();

        // Esta funci�n permite inyectar dependencias en el controlador
        // En este caso, inyectamos el logger para registrar informaci�n
        // y errores
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Lista simulando la base de datos
        private static List<UsuarioViewModel> _usuarios;

        static HomeController()
        {
            // Inicializar la lista con algunos datos de prueba
            _usuarios = new List<UsuarioViewModel>
                {
                    new UsuarioViewModel { Id = 1, Nombre = "Juan", Apellido = "P�rez", FechaNacimiento = new DateTime(1990, 1, 1), Correo = "juan.perez@example.com", Telefono = "12345678", Puesto = "Desarrollador", Direccion = "Calle Falsa 123", Estado = "Activo" },
                    new UsuarioViewModel { Id = 2, Nombre = "Mar�a", Apellido = "G�mez", FechaNacimiento = new DateTime(1985, 5, 15), Correo = "maria.gomez@example.com", Telefono = "87654321", Puesto = "Analista", Direccion = "Avenida Siempre Viva 456", Estado = "Activo" }
                };
        }

        public IActionResult Index()
        {
            return View(_usuarios);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
