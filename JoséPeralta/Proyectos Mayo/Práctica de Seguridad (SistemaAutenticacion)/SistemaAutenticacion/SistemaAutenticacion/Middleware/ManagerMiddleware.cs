using Newtonsoft.Json;
using System.Net;
using SistemaAutenticacion.Middleware;

namespace SistemaAutenticacion.Middleware
{
    public class ManagerMiddleware
    {

        // Declaramos las variables privadas para RequestDelegate y ILogger
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ManagerMiddleware> _logger;

        // Constructor de la clase que recibe un RequestDelegate y un ILogger
        public ManagerMiddleware(RequestDelegate requestDelegateNext, ILogger<ManagerMiddleware> logger)
        {
            _requestDelegate = requestDelegateNext;
            _logger = logger;
        }

        // Método que se ejecuta en cada petición HTTP
        public async Task ManagerExceptionAsync(HttpContext httpContext, Exception exception, ILogger<ManagerMiddleware> logger)
        {

            // Declaramos un object para almacenar la respuesta de error
            object? errorResponse = null;

            // Registramos el error en el logger
            switch (exception)
            {
                // Errores de tipo MiddlewareException
                case MiddleException middlewareException:
                    logger.LogError(exception, "MiddlewareException Error");
                    errorResponse = middlewareException.Errores;
                    httpContext.Response.StatusCode = (int)middlewareException.StatusCode;
                    break;

                case Exception e:
                    logger.LogError(exception, "Exception Error");
                    errorResponse = new
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = e.Message
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;

            }


            // Si no hay una respuesta de error, asignamos un mensaje genérico
            httpContext.Response.ContentType = "application/json"; // Establecemos el tipo de contenido de la respuesta a JSON

            var resultado = string.Empty; // Inicializamos una variable para almacenar el resultado de la respuesta

            // Si hay un errorResponse, lo convertimos a JSON y lo asignamos al resultado
            if (errorResponse != null)
            {
                // Convertimos el errorResponse a JSON
                resultado = JsonConvert.SerializeObject(new { errorResponse});
            }

            // Enviamos la respuesta de error al cliente
            await httpContext.Response.WriteAsync(resultado);

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Llamamos al siguiente middleware en la cadena de ejecución
                await _requestDelegate(httpContext);
            }
            catch (Exception e)
            {
                // Si ocurre una excepción, llamamos al método ManagerExceptionAsync para manejarla
                await ManagerExceptionAsync(httpContext, e, _logger);
            }
        }   



    }
}
