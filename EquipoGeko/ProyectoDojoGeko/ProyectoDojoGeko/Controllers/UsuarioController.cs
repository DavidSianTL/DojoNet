using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;

namespace ProyectoDojoGeko.Controllers
{
    public class UsuarioController : Controller
    {
        // Instanciamos el DAO
        private readonly daoUsuarioWSAsync _daoUsuarioWS;

        // Constructor para inicializar la cadena de conexión
        public UsuarioController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=localhost;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            // Inicializamos el DAO con la cadena de conexión
            _daoUsuarioWS = new daoUsuarioWSAsync(_connectionString);
        }

        public IActionResult Index()
        {
            return View();
        }




    }
}
