using System.Text;
using UsuariosApi.DAO;
using UsuariosApi.Models;



namespace UsuariosApi.Middleware
{
    public class AuditoriaMiddleware
    {


        private readonly RequestDelegate _next;
        private readonly daoAuditoria _daoAuditoria;
        private readonly daoAuditoriaEF _daoAuditoriaEF;
        private readonly ILogger<AuditoriaMiddleware> _logger;

        public AuditoriaMiddleware(RequestDelegate next, ILogger<AuditoriaMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext context, daoAuditoria daoAuditoria, daoAuditoriaEF daoAuditoriaEF)
        {
            // Detectar versión desde la ruta
            var path = context.Request.Path.ToString().ToLower();
            bool usarEF = path.StartsWith("/api/v5");
            


            var auditorDAO = usarEF ? (IAuditoriaDAO)_daoAuditoriaEF : _daoAuditoria;
            if (usarEF == true)
            {
                auditorDAO = daoAuditoriaEF;
            }
            else
            {
                auditorDAO = daoAuditoria;
            }
                var usuario = context.User.Identity?.IsAuthenticated == true
                    ? context.User.Identity.Name
                    : "Anónimo";

            var metodo = context.Request.Method;
            var ruta = context.Request.Path;
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var metodosAuditables = new[] { "POST", "PUT", "DELETE", "GET" };//, "PUT", "DELETE", "GET" 
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

                responseStatusCode = context.Response.StatusCode;

                if (responseStatusCode == 401)
                {
                    _logger.LogWarning("Acceso no autenticado a {Ruta} desde IP {IP}", ruta, ip);
                }
                else if (responseStatusCode == 403)
                {
                    _logger.LogWarning("Acceso denegado (no autorizado) a {Ruta} por usuario {Usuario}", ruta, usuario);
                }
                if (responseStatusCode == 401 || responseStatusCode == 403)
                {
                    mensajeError = $"Acceso {(responseStatusCode == 401 ? "no autenticado" : "no autorizado")}";
                }

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
                try 
                { 
                 await auditorDAO.InsertarAuditoriaAsync(auditoria);
                }
                catch { }

               
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
