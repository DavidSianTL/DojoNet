using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace ProyectoDojoGeko.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _rolesPermitidos;

        public AuthorizeRoleAttribute(params string[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var token = session.GetString("Token");
            var usuario = session.GetString("Usuario");
            var rolUsuario = session.GetString("Rol");

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

            // Verificar si el rol del usuario está en la lista de roles permitidos
            if (!string.IsNullOrEmpty(rolUsuario) && _rolesPermitidos.Contains(rolUsuario))
            {
                base.OnActionExecuting(context);
                return;
            }

            // Si no tiene permisos, redirigir a una página de acceso denegado
            context.Result = new RedirectToActionResult("AccesoDenegado", "Home", null);
        }

        private bool TokenHaExpirado(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);
                return jsonToken.ValidTo <= DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
        }
    }
}