using ClinicaMedicaAPIREST.Data.DAOs;
using ClinicaMedicaAPIREST.Data.DTO.AuthDTOs;
using ClinicaMedicaAPIREST.Models;
using ClinicaMedicaAPIREST.Security;

namespace ClinicaMedicaAPIREST.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequest);
    }

    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly daoUsuarios _daoUsuario;
        private readonly IJwtGenerator _jwtGenerator;
        public AuthService(ILogger<AuthService> logger, daoUsuarios daoUsuario, IJwtGenerator jwtGenerator)
        {
            _daoUsuario = daoUsuario;
            _logger = logger;
            _jwtGenerator = jwtGenerator;
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
        {            
            try
            {
                var usuario = await _daoUsuario.GetUsuarioByCredentialsAsync(loginRequest);
               
               
                // si el usuario es null => retornamos null (diferenciamos error de usuario invalido)
                if (usuario == null) return null!;

                var token = _jwtGenerator.GenerateToken(usuario);
                var tokenExpiration = DateTime.UtcNow.AddMinutes(_jwtGenerator.GetTokenExpiration());

                var usuarioValido = new AuthResponseDTO
                {
                    Username = usuario.Username,
                    Role = usuario.Role,
                    Token = token,
                    TokenExpiration = tokenExpiration
                };

                return usuarioValido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar iniciar sesión");
                return null!;
            }
        }
    }
}
