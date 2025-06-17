using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace SistemaAutenticacion.Middleware
{
    /// <summary>
    /// Escuchador de excepciones
    /// </summary>
    public class ManagerMiddleware
    {
        private readonly RequestDelegate _requestDelegateNext;
        private readonly ILogger<ManagerMiddleware> _logger;

        public ManagerMiddleware(RequestDelegate requestDelegateNext, ILogger<ManagerMiddleware> logger)
        {
            _requestDelegateNext = requestDelegateNext;
            _logger = logger;
        }

        private async Task ManagerExceptionAsync(HttpContext httpContext, Exception exception, ILogger<ManagerMiddleware> logger)
        {
            //Objeto que define el tipo de error generico
            object? errores = null;

            switch (exception)
            {
                //Errores de tipo MiddlewareException
                case MiddlewareException middlewareException:
                    logger.LogError(exception, "Middleware Error");
                    errores = middlewareException.Errores;
                    httpContext.Response.StatusCode = (int)middlewareException.Codigo;
                break;

                //Errores genericos
                case Exception ex:
                    logger.LogError(exception, "Error de servidor");
                    errores = string.IsNullOrEmpty(ex.Message) ? "Error": ex.Message;
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;

            }

            //Escribir el error en el cuerpo de la respuesta
            httpContext.Response.ContentType = "application/json";

            var resultado = string.Empty;

            if (errores != null)
            {
                resultado = JsonConvert.SerializeObject(new { errores });
            }

            //Enviar la respuesta al cliente
            await httpContext.Response.WriteAsync(resultado);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _requestDelegateNext(httpContext);
            }
            catch (Exception ex)
            {
                await ManagerExceptionAsync(httpContext, ex, _logger);
            }
        }

    }
}
