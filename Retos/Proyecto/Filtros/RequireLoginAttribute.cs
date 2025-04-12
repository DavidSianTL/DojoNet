using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Proyecto.Filters
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        private const string SessionUserId = "UsuarioId";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString(SessionUserId)))
            {
                // Redirigir al login en el controlador "Auth"
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
