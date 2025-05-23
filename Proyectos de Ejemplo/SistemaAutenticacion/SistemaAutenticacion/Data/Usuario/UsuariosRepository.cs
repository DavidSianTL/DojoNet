using Microsoft.AspNetCore.Identity;
using SistemaAutenticacion.Dtos.UsuarioDtos;
using SistemaAutenticacion.Models;
using SistemaAutenticacion.Token;

namespace SistemaAutenticacion.Data.Usuario
{
    public interface IUsuariosRepository
    {

    }
    public class UsuariosRepository: IUsuariosRepository
    {
        private readonly AppDbContext _context;
        private readonly IUsuarioSesion _usuarioSesion;
        private readonly UserManager<Usuarios> _userManager;
        private readonly SignInManager<Usuarios> _signInManager;
        private readonly IJwtGenerador _jwtGenerador;

        public UsuariosRepository(AppDbContext context, 
            IUsuarioSesion usuarioSesion, 
            UserManager<Usuarios> userManager, 
            SignInManager<Usuarios> signInManager, 
            IJwtGenerador jwtGenerador)
        {
            _context = context;
            _usuarioSesion = usuarioSesion;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerador = jwtGenerador;
        }

        private UsuarioResponseDto TransformerUserToUserDto(Usuarios user)
        {
            return new UsuarioResponseDto()
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Username = user.UserName,
                Token = _jwtGenerador.GenerarToken(user) //Creacion de token de seguridad
            };
        }

        //Obtener todos los usuarios
        public async Task<UsuarioResponseDto> GetUsuarios()
        {
            //Esta usuario cuenta con todas las propiedades del modelo del usuario y lo que se quiere
            //hacer es enviar solo las necesarias por lo que se deberia usar el modelo de UsuarioLoginRequestDto
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if (usuario is null)
            {
                throw new Exception("El token proporcionado no corresponde a ningun usuario registrado");
            }

            //Crear un UsuarioLoginRequestDto y asignarle solo las propiedades necesarias
            return TransformerUserToUserDto(usuario!);
        }



    }
}


