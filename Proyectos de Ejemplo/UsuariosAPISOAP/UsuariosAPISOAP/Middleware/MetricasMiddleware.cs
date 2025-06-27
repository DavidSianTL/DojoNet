using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using UsuariosAPISOAP.Data;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Middleware
{
   
    public class MetricasMiddleware
    {
        private readonly RequestDelegate _next;

        public MetricasMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            var stopwatch = Stopwatch.StartNew();
            var ruta = context.Request.Path;
            var metodo = context.Request.Method;
            var usuario = context.User?.Identity?.Name ?? "Anónimo";

            await _next(context); 

            stopwatch.Stop();

            var metric = new MetricasSolicitud
            {
                Fecha = DateTime.Now,
                Metodo = metodo,
                Ruta = ruta,
                Usuario = usuario,
                DuracionMilisegundos = stopwatch.ElapsedMilliseconds,
                CodigoRespuesta = context.Response.StatusCode
            };

            dbContext.Metricas.Add(metric);
            await dbContext.SaveChangesAsync();
        }
    }
}
