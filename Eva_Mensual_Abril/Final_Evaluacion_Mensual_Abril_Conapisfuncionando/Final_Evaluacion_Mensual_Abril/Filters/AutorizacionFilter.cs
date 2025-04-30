using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Final_Evaluacion_Mensual_Abril.Filters
{
    public class AutorizacionFilter : IActionFilter
    {
        private readonly List<string> _excludedActions = new List<string>
        {
            "Login/Login",
            "Login/NoAutorizado"
        };

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();
            var currentAction = $"{controller}/{action}";

            
            if (_excludedActions.Contains(currentAction))
                return;

            
            var usrNombre = context.HttpContext.Session.GetString("UsrNombre");

            if (usrNombre == null)
            {
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
