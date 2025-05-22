using Microsoft.AspNetCore.Identity;
using SistemaAutenticacion.Dtos.UsuarioDto;
using SistemaAutenticacion.Models;
using SistemaAutenticacion.Tokens;

namespace SistemaAutenticacion.Data.Usuario
{

    public interface IUsuariosRepository
    {

    }

    public class UsuariosRepository : IUsuariosRepository
    {

        // Inyección de dependencias
        private readonly AppDbContext _appDbContext;
        private readonly IUsuarioSesion _usuarioSesion;
        private readonly UserManager<UsuarioViewModel> _userManager;
        private readonly SignInManager<UsuarioViewModel> _signInManager;
        private readonly IJWTGenerator _jwtGenerator;

        // Constructor de la clase
        public UsuariosRepository
        (
            AppDbContext appDbContext,
            IUsuarioSesion usuarioSesion,
            UserManager<UsuarioViewModel> userManager,
            SignInManager<UsuarioViewModel> signInManager,
            IJWTGenerator jwtGenerator
        )
        {
            // Inicialización de las dependencias
            _appDbContext = appDbContext;
            _usuarioSesion = usuarioSesion;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;

        }

        // Método para obtener el usuario por su nombre de usuario
        private UsuarioResponseDto TransformerUserToUserDto(UsuarioViewModel user)
        {
            return new UsuarioResponseDto
            {
                // Se asignan los valores de las propiedades del usuario a las propiedades del DTO
                Id = user.Id,
                Nombres = user.Nombres,
                Apellidos = user.Apellidos,
                Email = user.Email,
                Telefono = user.Telefono,
                Token = _jwtGenerator.GenerateToken(user) // Generar el token de seguridad
            };
        }

        // Método para todos los usuarios
        public async Task<UsuarioResponseDto> GetUsuario()
        {
            // Este usuario cuenta con todas las propiedades de la clase (modelo) UsuarioViewModel
            // y solo se devuelven los datos que se necesitan
            var user = await _userManager.FindByIdAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if (user == null) 
            {
                throw new Exception("El token que se ha proporcionado, no corresponde a ningún usuario registrado");
            }

            // Retorna el usuario transformado a DTO
            return TransformerUserToUserDto(user!); // Se usa el operador de null-forgiving (!) para indicar que no es nulo
        }



    }
}
