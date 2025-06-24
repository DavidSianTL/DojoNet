namespace Trabajo_APIRest.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var usuario = context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anónimo";

            var metodo = context.Request.Method;
            var ruta = context.Request.Path;

            Console.WriteLine($"[{DateTime.Now}] {usuario} hizo {metodo} {ruta}");

            // Continuar con la siguiente etapa del pipeline
            await _next(context);
        }

    }
}
