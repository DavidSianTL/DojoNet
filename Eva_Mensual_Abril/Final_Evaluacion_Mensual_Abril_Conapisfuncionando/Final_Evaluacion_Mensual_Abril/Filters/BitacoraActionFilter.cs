using Microsoft.AspNetCore.Mvc.Filters;
using Final_Evaluacion_Mensual_Abril.Models;
using Final_Evaluacion_Mensual_Abril.Services;
using Proyecto1.Services;

namespace Final_Evaluacion_Mensual_Abril.Filters
{
    public class BitacoraActionFilter : IActionFilter
    {
        private readonly BitacoraService _bitacora;

        public BitacoraActionFilter(BitacoraService bitacora)
        {
            _bitacora = bitacora;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var httpContext = context.HttpContext;
            var usuario = httpContext.Session.GetString("UsrNombre") ?? "Anónimo";
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

           
            if (controller == "Bitacora") return;

            bool esError = context.Exception != null;

            var entry = new LogEntry
            {
                Usuario = usuario,
                Accion = action,
                Controlador = controller,
                Descripcion = esError ? $"Error: {context.Exception?.Message}" : "Acción ejecutada",
                EsError = esError
            };

            _bitacora.RegistrarEvento(entry);
        }
    }
}