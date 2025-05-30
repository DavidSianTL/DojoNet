using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace ProyectoDojoGeko.Filters
{
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var token = session.GetString("Token");
            var usuario = session.GetString("Usuario");
            var rol = session.GetString("Rol");

            // Verificar si existe token y usuario en sesión
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(usuario))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            // Verificar si el token ha expirado
            if (TokenHaExpirado(token))
            {
                // Limpiar sesión y redirigir al login
                session.Clear();
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            base.OnActionExecuting(context);
        }

        private bool TokenHaExpirado(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);

                // Verificar si el token ha expirado
                return jsonToken.ValidTo <= DateTime.UtcNow;
            }
            catch
            {
                return true; // Si hay error, consideramos que expiró
            }
        }


    }
}