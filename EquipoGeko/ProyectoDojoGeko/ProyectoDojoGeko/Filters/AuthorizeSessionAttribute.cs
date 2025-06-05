using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace ProyectoDojoGeko.Filters
{
    // Método de filtro de acción para autorizar sesiones
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        // Método que se ejecuta antes de la acción del controlador
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            // Verificar si la sesión está disponible
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

            // Si el token es válido, continuamos con la ejecución de la acción
            base.OnActionExecuting(context);
        }

        // Método para verificar si el token ha expirado
        private bool TokenHaExpirado(string token)
        {
            try
            {
                // Intentamos leer el token
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