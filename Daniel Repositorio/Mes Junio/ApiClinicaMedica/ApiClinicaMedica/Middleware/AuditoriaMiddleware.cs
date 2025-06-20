namespace ApiClinicaMedica.Middleware
{
    public class AuditoriaMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditoriaMiddleware(RequestDelegate next)
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
            var ip = context.Connection.RemoteIpAddress?.ToString();

            Console.WriteLine($"[{DateTime.Now}] {usuario} hizo {metodo} en {ruta} desde IP {ip}");

            await _next(context);
        }
    }
}
