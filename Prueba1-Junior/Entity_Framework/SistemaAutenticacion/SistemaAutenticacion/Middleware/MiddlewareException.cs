using System.Net;

namespace SistemaAutenticacion.Middleware
{
    public class MiddlewareException: Exception
    {
        public HttpStatusCode Codigo { get; set; }
        public object? Errores { get; set; }

        public MiddlewareException(HttpStatusCode codigo, object? errores)
        {
            Codigo = codigo;
            Errores = errores;
        }
    }
}
