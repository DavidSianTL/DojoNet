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
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ManagerMiddleware> _logger;

        public ManagerMiddleware(RequestDelegate requestDelegateNext, ILogger<ManagerMiddleware> logger)
        {
            _requestDelegate = requestDelegateNext;
            _logger = logger;
        }

        private async Task ManagerExceptionAsync(HttpContext httpContext, Exception exception, ILogger<ManagerMiddleware> logger)
        {
            //Objeto que define el tipo de error generico
            object? errores = null;

            switch (exception)
            {
                //Errores de tipo MiddlewareException
                case MiddleException middlewareException:
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
                // Llamamos al siguiente middleware en la cadena de ejecución
                await _requestDelegate(httpContext);

            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, llamamos al método ManagerExceptionAsync para manejarla
                await ManagerExceptionAsync(httpContext, ex, _logger);
            }
        }

    }
}
