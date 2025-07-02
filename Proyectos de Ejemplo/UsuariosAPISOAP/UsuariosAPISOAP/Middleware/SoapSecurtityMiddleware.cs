
// Middleware/SoapSecurityMiddleware.cs
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace UsuariosAPISOAP.Middleware
{
    public class SoapSecurityMiddleware
    {
        private readonly RequestDelegate _next;

        public SoapSecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Solo revisamos si es una llamada SOAP POST
            if (context.Request.ContentType != null &&
                context.Request.ContentType.Contains("text/xml") &&
                context.Request.Method == "POST")
            {
                context.Request.EnableBuffering(); // Permite leer cuerpo mas d euna vez

                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Volvemos al inicio para que SoapCore lo lea

                if (body.Length > 1_000_000 ||           // Muy grande
                    body.Contains("<!ENTITY") ||         // Entidades externas
                    CountOccurrences(body, "&") > 1000)  // Posible bomba
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("XML SOAP inválido o potencialmente malicioso.");
                    return;
                }
            }

            await _next(context);
        }

        private int CountOccurrences(string input, string pattern)
        {
            int count = 0, index = 0;
            while ((index = input.IndexOf(pattern, index)) != -1)
            {
                count++;
                index += pattern.Length;
            }
            return count;
        }



    }
}
