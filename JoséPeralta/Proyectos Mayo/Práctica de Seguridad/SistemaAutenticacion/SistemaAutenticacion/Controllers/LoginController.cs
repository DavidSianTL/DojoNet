using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaAutenticacion.Data.Usuario;
using SistemaAutenticacion.Dtos.UsuarioDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioResponseDto>> Login(UsuarioLoginRequestDto loginRequest)
        {
            var usuario = await _usuariosRepository.Login(loginRequest); // usa tu lógica actual

            return RedirectToAction("Index", "Home"); // Vista protegida
        }

        // GET: Login/Usuarios
        [HttpGet]
        public async Task<ActionResult<UsuarioResponseDto>> Usuarios()
        {
            return await _usuariosRepository.GetUsuario();
        }

        // POST: Login/RegistroUsuario
        [AllowAnonymous]
        [HttpPost("RegistroUsuario")]
        public async Task<ActionResult<UsuarioResponseDto>> RegistroUsuario(UsuarioRegistroRequestDto usuarioRegistro)
        {
            return await _usuariosRepository.Registrar(usuarioRegistro);
        }

    }
}
