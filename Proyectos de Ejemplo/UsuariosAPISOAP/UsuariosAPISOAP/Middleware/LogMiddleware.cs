using UsuariosAPISOAP.Data;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            var ruta = context.Request.Path;
            var metodo = context.Request.Method;
            var usuario = context.User?.Identity?.Name ?? "Anónimo";

            var log = new LogEvento
            {
                Fecha = DateTime.Now,
                Ruta = ruta,
                Metodo = metodo,
                Usuario = usuario,
                Mensaje = "Solicitud procesada"
            };

            dbContext.LogEventos.Add(log);
            await dbContext.SaveChangesAsync();

            await _next(context);

        }
    }
}
