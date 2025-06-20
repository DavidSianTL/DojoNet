using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaAutenticacion.Data.Usuario;
using SistemaAutenticacion.Dtos.UsuarioDtos;

namespace SistemaAutenticacion.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuariosRepository _usuariosRepository;

        public LoginController(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioResponseDto>> ObtenerUsuarioSesion()
        {
           return await _usuariosRepository.GetUsuarios();
        }

        [AllowAnonymous]
        [HttpPost("RegistroUsuario")]
        public async Task<ActionResult<UsuarioResponseDto>> RegistroUsuario(UsuarioRegistroRequestDto usuarioRegistro)
        {
            return await _usuariosRepository.Registrar(usuarioRegistro);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioResponseDto>> Login(UsuarioLoginRequestDto loginRequest)
        {
            var usuario = await _usuariosRepository.Login(loginRequest); // usa tu lógica actual

            return RedirectToAction("Index", "Home"); // Vista protegida
        }

    }
}
