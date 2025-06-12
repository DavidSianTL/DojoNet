using System.Text;
using UsuariosApi.DAO;
using UsuariosApi.Models;



namespace UsuariosApi.Middleware
{
    public class AuditoriaMiddleware
    {


        private readonly RequestDelegate _next;

        public AuditoriaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, daoAuditoria daoAuditoria)
        {
            var usuario = context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anónimo";

            var metodo = context.Request.Method;
            var ruta = context.Request.Path;
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var metodosAuditables = new[] { "POST"};//, "PUT", "DELETE", "GET" 
            if (!metodosAuditables.Contains(metodo))
            {
                await _next(context);
                return;
            }


            var responseStatusCode = 0;
            string mensajeError = "";

            string cuerpo = null;
            string tipoAccion = DetectarTipoAccion(metodo, ruta);

            // Leer cuerpo (si existe)
            if (context.Request.ContentLength > 0 &&
                context.Request.ContentType != null &&
                context.Request.ContentType.Contains("application/json"))
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                cuerpo = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset para el resto de la app
            }

            try
            {
                // Procesar siguiente middleware
                await _next(context);
                responseStatusCode = context.Response.StatusCode;
            }
            catch (Exception ex)
            {
                // Error en la ejecución
                responseStatusCode = 500;
                mensajeError = ex.Message;
                throw;
            }
            finally
            {
                var auditoria = new Auditoria
                {
                    Usuario = usuario,
                    Metodo = metodo,
                    Ruta = ruta,
                    IpOrigen = ip,
                    Estado = responseStatusCode,
                    Mensaje = mensajeError,
                    Cuerpo = cuerpo,
                    TipoAccion = tipoAccion
                };

                await daoAuditoria.InsertarAuditoriaAsync(auditoria);
            }
        }


        private string DetectarTipoAccion(string metodo, string ruta)
        {
            ruta = ruta.ToLower();

            if (ruta.Contains("login"))
                return "Login";
            if (ruta.Contains("usuarios") && metodo == "POST")
                return "Creación de usuario";
            if (ruta.Contains("usuarios") && metodo == "PUT")
                return "Modificación de usuario";
            if (ruta.Contains("usuarios") && metodo == "DELETE")
                return "Eliminación de usuario";

            if (ruta.Contains("usuarios") && metodo == "GET")
                return "Obtenciòn de usuario";

            return "Operación general";
        }
    }
}
