using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Dtos.UsuarioDtos;
using SistemaAutenticacion.Middleware;
using SistemaAutenticacion.Models;
using SistemaAutenticacion.Token;
using System.Net;

namespace SistemaAutenticacion.Data.Usuario
{
    public interface IUsuariosRepository
    {
        Task<UsuarioResponseDto> GetUsuarios();
        Task<UsuarioResponseDto> LoginUsuario(UsuarioLoginRequestDto usuarioLogin);
        Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto usuarioRegistro);
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
                //throw new Exception("El token proporcionado no corresponde a ningun usuario registrado");
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El token proporcionado no corresponde a ningun usuario registrado" });
            }

            //Crear un UsuarioLoginRequestDto y asignarle solo las propiedades necesarias
            return TransformerUserToUserDto(usuario!);
        }

        //Metodo login 
        public async Task<UsuarioResponseDto> LoginUsuario(UsuarioLoginRequestDto usuarioLogin)
        {
            //Metodo para Usuarios que ya estan dentro de la sesion
            var usuario = await _userManager.FindByEmailAsync(usuarioLogin.Email!);

            if (usuario is null)
            {
                //throw new Exception("No se encontro una cuenta asociada con el correo electronico ingresado");
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "No se encontro una cuenta asociada con el correo electronico ingresado" });
            }

            var Resultado = await _signInManager.CheckPasswordSignInAsync(usuario, usuarioLogin.Password!, false);

            if (Resultado.Succeeded)
            {
                return TransformerUserToUserDto(usuario!);
            }

            //throw new Exception("Las credenciales proporcionadas son incorrectas. Por favor, verifica tu correo y contraseña");
            throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "Las credenciales proporcionadas son incorrectas. Por favor, verifica tu correo y contraseñ" });
        }


        //Metodo registro
        public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto usuarioRegistro)
        {
            //Si existe el email
            var emailExiste = await _context.Users.Where(x => x.Email == usuarioRegistro.Email).AnyAsync();

            if (emailExiste)
            {
                //throw new Exception("El correo electronico ya se encuentra registrado. Intenta iniciar sesión o recuperar tu contraseña.");
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El correo electronico ya se encuentra registrado. Intenta iniciar sesión o recuperar tu contraseña." });
            }

            //Si existe el username
            var usernameExiste = await _context.Users.Where(x => x.UserName == usuarioRegistro.Username).AnyAsync();

            if (usernameExiste)
            {
                //throw new Exception("El nombre de usuario ya se encuentra registrado. Intenta iniciar sesión o recuperar tu contraseña.");
                throw new MiddlewareException(HttpStatusCode.BadRequest, new { mensaje = "El nombre de usuario ya se encuentra registrado. Intenta iniciar sesión o recuperar tu contraseña." });
            }

            //Crear un nuevo usuario
            var Usuario = new Usuarios()
            {
                Nombre = usuarioRegistro.Nombre,
                Apellido = usuarioRegistro.Apellido,
                Email = usuarioRegistro.Email,
                UserName = usuarioRegistro.Username,
                Telefono = usuarioRegistro.Telefono,
                FechaCreacion = usuarioRegistro.FechaCreacion = DateTime.UtcNow
            };

            var Resultado = await _userManager.CreateAsync(Usuario, usuarioRegistro.Password!);

            if (Resultado.Succeeded)
            {
                return TransformerUserToUserDto(Usuario!);
            }

            throw new Exception("No se pudo crear el usuario. Por favor, verifica los datos ingresados y vuelve a intentarlo.");

        }


        //Funcion centrada unicamente en obtener el rol del usuario que esta en sesion, Vendedor, Administrador, etc.
        //Retorna el primer rol asignado al usuario
        public async Task<string> ObtenerRolUsuarioSesion()
        {
            //Obtener el nombre del usuario que esta en sesion
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if (usuario is null)
            {
                //throw new Exception("El usuario no esta autenticado");
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El usuario no esta autenticado" });
            }

            //buscar roles
            var roles = await _userManager.GetRolesAsync(usuario);

            //Si el usuario tiene algun rol, devolver el primer rol asignado
            if (roles.Any())
            {
                return roles.First();
            }

            //throw new Exception("El usuario no tiene roles asignados");
            throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = "El usuario no tiene roles asignados" });

        }

    }
}


