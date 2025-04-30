using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class LoginAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var session = context.HttpContext.Session;
        var usuario = session.GetString("UsrNombre");

        
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        if (string.Equals(controllerName, "Login", StringComparison.OrdinalIgnoreCase))
        {
            base.OnActionExecuting(context);
            return;
        }

        if (string.IsNullOrEmpty(usuario))
        {
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Login" },
                    { "action", "Login" }
                });
        }

        base.OnActionExecuting(context);
    }
}
