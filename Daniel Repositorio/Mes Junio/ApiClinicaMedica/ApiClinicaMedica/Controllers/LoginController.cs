using Microsoft.AspNetCore.Mvc;
using ApiClinicaMedica.Models;
using ApiClinicaMedica.Services;


namespace ApiClinicaMedica.Controllers
{
    [ApiController]
    [Route("api/v4/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public LoginController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            // Simulación: usuario hardcodeado
            if (request.Usuario == "admin" && request.Contrasenia == "1234")
            {
                var token = _jwtService.GenerateToken(request.Usuario);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Credenciales inválidas" });
        }
    }
}
