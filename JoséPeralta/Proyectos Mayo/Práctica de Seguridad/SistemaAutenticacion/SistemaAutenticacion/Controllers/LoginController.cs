using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaAutenticacion.Data.Usuario;
using SistemaAutenticacion.Dtos.UsuarioDto;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SistemaAutenticacion.Controllers
{
    public class LoginController : Controller
    {
        // Repositorio para manejar las operaciones de usuario
        private readonly IUsuariosRepository _usuariosRepository;

        // Constructor que recibe el repositorio de usuarios
        public LoginController(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        // GET: Login/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Login/Usuarios
        [HttpGet]
        public async Task<ActionResult<UsuarioResponseDto>> Usuarios()
        {
            return await _usuariosRepository.GetUsuario();
        }



    }
}
