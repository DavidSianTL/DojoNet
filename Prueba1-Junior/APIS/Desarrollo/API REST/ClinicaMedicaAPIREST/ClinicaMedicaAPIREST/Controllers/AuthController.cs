using ClinicaMedicaAPIREST.Data.DTO.AuthDTOs;
using ClinicaMedicaAPIREST.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedicaAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await _authService.LoginAsync(loginRequest);

            if (user == null) return Unauthorized("Credenciales inválidas");

            return Ok(user);
        }
    }
}
