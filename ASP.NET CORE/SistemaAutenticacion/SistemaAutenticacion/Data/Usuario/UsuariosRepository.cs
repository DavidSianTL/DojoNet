using Microsoft.AspNetCore.Identity;
using SistemaAutenticacion.Token;
using SistemaAutenticacion.Models;
using SistemaAutenticacion.Dtos.UsuarioDtos;

namespace SistemaAutenticacion.Data.Usuario
{
    //Clase base del usuario CRUD
    public interface IUsuarioRepository
    {

    }
    //Creamos una clase que implementa la interface de arriba
    public class UsuariosRepository: IUsuarioRepository
    {
        //Son las dependencias que tienen la lógica de otras clases 

        //permite la conexión a la BD
        private readonly AppDbContext _context;
        //
        private readonly IUsuarioSesion _usuarioSesion;
        //permite realizar diferentes acciones CRUd
        private readonly UserManager<Usuarios> _userManager;//Asegurarse que se esta llamando al modelo Usuario
        //permite manejar el inicio de sesion de usuario
        private readonly SignInManager<Usuarios> _signInManager;
        //permite crear el token del usuario
        private readonly IJwtGenerador _jwtGenerador;


        public UsuariosRepository(AppDbContext context, IUsuarioSesion usuarioSesion, UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, IJwtGenerador jwtGenerador)
        {
           //nos deja utilizar las dependencias anteriores para usarlo en la clase
            _context = context;
            _usuarioSesion = usuarioSesion;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerador = jwtGenerador;
        }

        //Transforma el usuario en un usuario DTO
        private UsuarioResponseDto TransformerUserToUserDto(Usuarios user)
        {
            //cuando un usuario hace login, se le devuelve estos datos
            //Permite tener un estandar para que no se envien datos sensibles al usuario y permite escalabilidad
            return new UsuarioResponseDto()
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Username = user.UserName,
                Token = _jwtGenerador.GenerarToken(user) //Creacion de Token de seguridad
            };
        }

        //Sirve para obtener todos los usuarios
        //LLamamos al UsuarioResponseDto porque es lo que vamos a mostrar al usuario
        public async Task<UsuarioResponseDto> GetUsuarios()
        {
            //Este usuario cuenta con todas las propiedades del usuario y lo que se quiere hacer es enviar solo las necesarias por lo que
            //se debería usar el modelo UsuarioLoginRequestDto
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if(usuario is null)
            {
                //Forma sencilla de tirar una excepcion
                throw new Exception("El token proporcionado no corresponde a ningún usuario registrado");

            }

            //Creamos un UsuarioLoginRequestDto y asignarle solo las propiedades necesarias.
            return TransformerUserToUserDto(usuario!);
        }
    }
}
