using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using AppFuturista.Services;

namespace AppFuturista.Filters
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = context.HttpContext.Session.GetString("UsuarioId");
            
            if (string.IsNullOrEmpty(usuarioId))
            {
                // Si no hay sesión activa, redirigir al login
                context.Result = new RedirectToActionResult("Login", "Cuenta", null);
                return;
            }
            
            // Registrar el acceso a la ruta en la bitácora
            var bitacoraService = context.HttpContext.RequestServices.GetService(typeof(BitacoraService)) as BitacoraService;
            var nombreUsuario = context.HttpContext.Session.GetString("NombreUsuario") ?? "Usuario desconocido";
            var ruta = context.HttpContext.Request.Path;
            
            Task.Run(async () => 
            {
                try
                {
                    await bitacoraService.RegistrarEventoAsync("Navegación", $"Usuario: {nombreUsuario} - Acceso a: {ruta}");
                }
                catch
                {
                    // Ignorar errores en el filtro para no interrumpir la navegación
                }
            });
            
            base.OnActionExecuting(context);
        }
    }
}
