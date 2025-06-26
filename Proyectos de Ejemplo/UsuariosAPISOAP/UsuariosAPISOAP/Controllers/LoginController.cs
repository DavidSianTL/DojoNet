using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosAPISOAP.Data;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            
            var user = _context.UsuariosEF.FirstOrDefault(u => u.usuario == request.Usuario);

            if (user == null || (request.Contrasenia != user.contrasenia))
            {
                return Unauthorized("Credenciales incorrectas");
            }

            /*if (user == null || !BCrypt.Net.BCrypt.Verify(request.Contrasenia, user.contrasenia))
            {
                return Unauthorized("Credenciales incorrectas");
            }*/
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.usuario),
            new Claim("UsuarioId", user.id_usuario.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("W1(1@V3sVP3RS3(437@P@4W1(1@V3sVP")); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "UsuariosAPISOAP",
                audience: "SessionUsuariosAPISOAP",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


            return Ok(new { token = tokenString } );
        }
    }
}
