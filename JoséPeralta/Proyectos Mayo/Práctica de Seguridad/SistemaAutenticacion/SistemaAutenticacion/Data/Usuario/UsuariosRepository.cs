using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Dtos.UsuarioDto;
using SistemaAutenticacion.Models;
using SistemaAutenticacion.Tokens;

namespace SistemaAutenticacion.Data.Usuario
{

    public interface IUsuariosRepository
    {

        // Método para obtener el usuario en sesión
        Task<UsuarioResponseDto> GetUsuario();
        // Método para iniciar sesión
        Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto usuarioLogin);
        // Método para registrar un nuevo usuario
        Task<UsuarioResponseDto> Registrar(UsuarioRegistroRequestDto usuarioRegistro);
        // Método para obtener el rol del usuario en sesión
        Task<string> ObtenerRolUsuarioEnSesion();

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

        // Método de inicio de sesión
        public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto usuarioLogin)
        {

            // Creamos un método para obtener el usuario en sesión
            var usuario = await _userManager.FindByEmailAsync(usuarioLogin.Email!);

            // Validamos que el usuario exista
            if (usuario == null)
            {
                throw new Exception("No se encontró una cuenta asociada a este correo: " + usuario.Email);
            }

            // Validamos que la contraseña sea correcta
            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, usuarioLogin.Password!, false);

            // Si el resultado es exitoso, se genera el token y se retorna el usuario transformado a DTO
            if (resultado.Succeeded)
            {
                return TransformerUserToUserDto(usuario!);
            }

            // Si el resultado no es exitoso, se lanza una excepción
            throw new Exception("Credenciales incorrectas. Por favor, verifique que su correo sea correcto al igual que su contraseña...");

        }

        // Método para registrar un nuevo usuario
        public async  Task<UsuarioResponseDto> Registrar(UsuarioRegistroRequestDto usuarioRegistro)
        {

            // Validamos que el correo no exista usando el "_appDbContext" para consultar la base de datos
            var emailExistente = await _appDbContext.Users.Where(x => x.Email == usuarioRegistro.Email).AnyAsync();

            // Si el usuario ya existe, se lanza una excepción
            if (emailExistente)
            {
                throw new Exception("Ya existe un usuario registrado con este correo: " + usuarioRegistro.Email);
            }

            // Creamos un nuevo usuario con los datos del registro
            var usuario = new UsuarioViewModel
            {
                Nombres = usuarioRegistro.Nombres,
                Apellidos = usuarioRegistro.Apellidos,
                Email = usuarioRegistro.Email,
                UserName = usuarioRegistro.Email, // Usamos el email como nombre de usuario
                Telefono = usuarioRegistro.Telefono,
                FechaCreacion = DateTime.UtcNow // Usamos el UtcNow para referirnos a la fecha y hora actual en formato UTC(hora universal coordinada)
            };

            // Agregamos el usuario a la base de datos
            var resultado = await _userManager.CreateAsync(usuario, usuarioRegistro.Password!);

            // Si el resultado es exitoso, se genera el token y se retorna el usuario transformado a DTO
            if (resultado.Succeeded)
            {
                return TransformerUserToUserDto(usuario);
            }

            // Si el resultado no es exitoso, se lanza una excepción con los errores
            throw new Exception("Error al registrar el usuario. Por favor, verifica los datos ingresados e intenta de nuevo.");

        }

        // Método para obtener el rol del usuario que está en sesión (Vendedor, Administrador, etc.)
        // Retornamos el primer rol que el usuario tenga asignado
        public async Task<string> ObtenerRolUsuarioEnSesion()
        {

            // Obtener el usuario en sesión
            var usuario = await _userManager.FindByIdAsync(_usuarioSesion.ObtenerUsuarioSesion());

            // Validamos que el usuario exista
            if (usuario is null)
            {
                throw new Exception("El token que se ha proporcionado, no corresponde a ningún usuario en sesión");
            }

            // Obtenemos los roles del usuario
            var roles = await _userManager.GetRolesAsync(usuario);

            // Validamos que el usuario tenga al menos un rol
            if (roles.Count == 0)
            {
                throw new Exception("El usuario no tiene roles asignados.");
            }

            // Retornamos el primer rol del usuario
            return roles.FirstOrDefault(); // Si no hay roles


        }


    }
}
