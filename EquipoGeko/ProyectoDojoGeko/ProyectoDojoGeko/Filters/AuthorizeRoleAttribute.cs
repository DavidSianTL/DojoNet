using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace ProyectoDojoGeko.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        // Array de roles permitidos
        private readonly string[] _rolesPermitidos;

        // Constructor que recibe los roles permitidos como parámetros
        public AuthorizeRoleAttribute(params string[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        // Método que se ejecuta antes de la acción del controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verificar si la sesión está disponible
            var session = context.HttpContext.Session;
            var token = session.GetString("Token");
            var usuario = session.GetString("Usuario");
            var rolesUsuario = session.GetString("Roles");

            // Verificar si existe token y usuario en sesión
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(usuario))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Verificar si el token ha expirado
            if (TokenHaExpirado(token))
            {
                session.Clear();
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Verificar si los roles del usuario estan en la lista de roles permitidos
            if (!string.IsNullOrEmpty(rolesUsuario))
            {
                var rolesDelUsuario = rolesUsuario.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (rolesDelUsuario.Any(r => _rolesPermitidos.Contains(r)))
                {
                    base.OnActionExecuting(context);
                    return;
                }
            }

            // Si no tiene permisos, redirigir a una página de acceso denegado
            context.Result = new RedirectToActionResult("AccesoDenegado", "Home", null);
        }

        // Creamos un método para verificar si el token ha expirado
        private bool TokenHaExpirado(string token)
        {
            try
            {
                // Intentamos leer el token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);

                // Verificamos si la fecha de expiración es menor o igual a la fecha actual
                return jsonToken.ValidTo <= DateTime.UtcNow;
            }
            catch
            {
                // Si ocurre un error al leer el token, asumimos que ha expirado
                return true;
            }
        }
    }
}