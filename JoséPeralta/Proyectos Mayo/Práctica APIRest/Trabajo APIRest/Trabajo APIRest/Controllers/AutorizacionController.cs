
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Services;
using Trabajo_APIRest.Dtos.UsuarioDtos;
using UsuariosApi.DAO;

namespace UsuariosApi.Controllers.v5
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutorizacionController : Controller
    {
        private readonly daoUsuariosWSAsync _daoUsuarioAsync;
        private readonly JwtService _jwtService;
        private readonly daoUsuariosWSAsync _daoUsuario;

        public AutorizacionController(daoUsuariosWSAsync daoUsuarioAsync, JwtService jwtService, daoUsuariosWSAsync daoUsuario)
        {
            _daoUsuarioAsync = daoUsuarioAsync;
            _jwtService = jwtService;
            _daoUsuario = daoUsuario;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequestDto request)
        {
            // Traemos todos los usuarios de la base de datos
            var usuarios = await _daoUsuarioAsync.ObtenerUsuariosAsync();

            // Verificamos si el usuario y la contraseña coinciden con alguno de los usuarios
            foreach (var u in usuarios)
            {
                // Si encontramos un usuario con las credenciales correctas
                if (u.Usuario == request.Usuario && u.Contrasenia == request.Contrasenia)
                {

                    // Generamos el token JWT
                    var token = _jwtService.GenerateToken(u.Usuario);

                    // Actualizamos el token en la base de datos
                    _daoUsuario.ActualizarToken(u.IdUsuario, token);

                    // Devolvemos el token y el usuario
                    return Ok(new UsuarioResponseDto
                    {
                        Token = token,
                        Usuario = u.Usuario
                    });
                }
            }

            // Si no encontramos un usuario con las credenciales correctas, devolvemos un error 401 Unauthorized
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });

        }
    }
}
