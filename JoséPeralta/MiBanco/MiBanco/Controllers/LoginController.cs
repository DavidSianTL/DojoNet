using MiBanco.Data;
using MiBanco.Services;
using Microsoft.AspNetCore.Mvc;
using MiBanco.Dto.Login.Request;


namespace MiBanco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {

        private readonly daoUsuarios _daoUsuarios;
        private readonly JWTService _jwtService;
        private readonly IBitacoraService _bitacoraService;

        // Constructor que recibe el daoPagos, daoCuentas y IBitacoraService para inyección de dependencias
        public LoginController(daoUsuarios daoUsuarios, JWTService jwtService, IBitacoraService bitacoraService)
        {
            _daoUsuarios = daoUsuarios;
            _jwtService = jwtService;
            _bitacoraService = bitacoraService;
        }

        // Método para iniciar sesión
        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest request) // Con FromBody le indicamos que el cuerpo de la solicitud contendrá los datos del usuario y contraseña
        {

            // Validamos que el usuario y la contraseña no estén vacíos
            if(request.Usuario == null || request.Contraseña == null){
                return BadRequest("Usuario y contraseña son requeridos.");
            }

            // Verificamos las credenciales del usuario
            var usuarioEncontrado = _daoUsuarios.VerificarCredenciales(request.Usuario, request.Contraseña);

            // Si el usuario no fue encontrado, retornamos un error 404
            if (usuarioEncontrado == null)
            {
                return NotFound("Usuario o contraseña incorrectos.");
            }

            // Registramos la acción en la bitácora
            _bitacoraService.RegistrarAccion("Login", $"Login exitoso para: {usuarioEncontrado.Usuario}");

            // Generamos un token de sesión para el usuario que se ha logeado
            var token = _jwtService.GenerarToken(usuarioEncontrado.Id, usuarioEncontrado.Usuario);

            // Retornamos un mensaje de éxito
            //return Ok($"Inicio de sesión exitoso. Token: {token}");

            return Ok(new
            {
                mensaje = "Inicio de sesión exitoso.",
                token = token
            });

        }


    }
}
